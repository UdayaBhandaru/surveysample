import { Component, Input, Output, EventEmitter, ChangeDetectorRef, ViewChild } from "@angular/core";

import { HttpClient } from "@angular/common/http";
import { FormGroup, FormControl } from "@angular/forms";
import { Observable, Subscription } from "rxjs";;
import { map, debounceTime, startWith } from "rxjs/operators";
import { MatAutocompleteTrigger } from "@angular/material";
@Component({
    selector: "autocomplete",
    template: `<mat-form-field [formGroup]="formGroup" class="{{cssClass}} material-auto-complete">
    <input matInput type="text" placeholder="{{placeholder}}"
           [formControl]="formControl" [matAutocomplete]="auto" [required]="required" #autoCompleteElement/>
<div class="autocomplte-search-icon retailsNavIcons"></div>
</mat-form-field>
<mat-autocomplete #auto="matAutocomplete" [displayWith]="displayFn.bind(this)" (optionSelected)="onOptionSelect($event)">
    <div *ngIf="isDataFromApiOnValueChange">
    <mat-option *ngFor="let option of filteredOptions" [value]="option[optionKey]" >
     <span style='text-align: center'><img *ngIf='option[imageSrc]' [src]='option[imageSrc]'  [class]='imageClass'/></span>
    <span  style='text-align: center'>{{option[optionValue]}}</span>
    </mat-option>
    </div>
    <div *ngIf="!isDataFromApiOnValueChange">
    <mat-option *ngFor="let option of filteredOptions | async" [value]="option[optionKey]">
     <span style='text-align: center'><img *ngIf='option[imageSrc]' [src]='option[imageSrc]'  [class]='imageClass'/></span>
     <span  style='text-align: center'>{{option[optionValue]}}</span>
    </mat-option>
    </div>
</mat-autocomplete>`
})
export class CommonAutoCompleteComponent {
    @Input() formGroup: FormGroup;
    @Input() formControl: FormControl;
    @Input() required: boolean;
    @Input() placeholder: string;
    @Input() optionsData: any;
    @Input() optionKey: any;
    @Input() optionValue: any;
    @Input() cssClass: string;
    @Input() imageSrc: any;
    @Input() imageClass: any;
    @Input() minLength: number = 0;
    @Input() apiUrl: string;
    @Input() disabled: boolean;
    isDataFromApiOnValueChange: boolean;
    asyncString: string = " | async ";
    private filteredOptions: Observable<any>;
    @Output() fxChange = new EventEmitter();
    @ViewChild(MatAutocompleteTrigger) trigger: MatAutocompleteTrigger;
    @ViewChild("autoCompleteElement") autoCompleteElement: any;
    private subscription: Subscription;
    constructor(private ref: ChangeDetectorRef, private http: HttpClient) {

    }

    ngOnInit(): void {
        let self: CommonAutoCompleteComponent = this;
        self.formControl.enable();
        if (this.disabled) {
            self.formControl.disable();
        }
        if (self.apiUrl) {
            self.isDataFromApiOnValueChange = true;
        }
        if (self.isDataFromApiOnValueChange) {
            self.asyncString = null;
            self.setApiToAutoComplete();
        } else {
            self.filteredOptions = self.formControl.valueChanges
                .pipe(startWith(null),
                map(x => x ? self.filter(x) : (self.optionsData ? self.optionsData.slice() : null)));
        }
    }

    ngAfterViewChecked(): void {
        var self: CommonAutoCompleteComponent = this;
        this.ref.detach();
        this.ref.detectChanges();
        if (!self.isDataFromApiOnValueChange) {
            if (!self.formControl.value) {
                self.filteredOptions = self.formControl.valueChanges
                    .pipe(startWith(null),
                    map(function (x: any): any { return x ? self.filter(x) : (self.optionsData ? self.optionsData.slice() : null); }));
            }
        }
        if (self.formControl && self.disabled) {
            this.formControl.disable();
        }
    }

    ngAfterViewInit(): void {
        this.subscribeToClosingActions();
    }

    ngOnDestroy(): void {
        if (!this.subscription && !this.subscription.closed) {
            this.subscription.unsubscribe();
        }
    }

    private setApiToAutoComplete(): void {
        let self: CommonAutoCompleteComponent = this;
        self.formControl.valueChanges.pipe(debounceTime(400)).subscribe((word) => {
            if (word && word.length >= this.minLength) {
                this.getDataFromAPI(word).subscribe(response => {
                    self.filteredOptions = response;
                    self.optionsData = self.filteredOptions;
                });
            } else {
                self.filteredOptions = null;
            }
        }
        );
    }

    private getDataFromAPI(word: string): Observable<any> {
      var resp = this.http.get(this.apiUrl + word);
        //var resp: any = this.http.get(this.apiUrl + word).map((resp: Response) => {
        //    return resp.json();
        //});
        return resp;
    }

  public filter(val: string): any {
        let self: CommonAutoCompleteComponent = this;
        return val ? self.optionsData.filter((option: any) => new RegExp(`^${val}`, "gi").test(option[self.optionValue])) : self.optionsData;
    }

  public displayFn(value: any): any {
        return value ? this.getDisplayValue(value) : value;
    }

    private getDisplayValue(val: any): any {
        let self: CommonAutoCompleteComponent = this;
        if (self.optionsData) {
            var displayValue: any = self.optionsData.filter((option: any) => option[self.optionKey] === val)[0][self.optionValue];
            return displayValue;
        }

        return null;
    }

  public onOptionSelect(event: any): any {
        let self: CommonAutoCompleteComponent = this;
        let option: any = this.optionsData.find(x => x[this.optionKey] === event.option.value);
        self.fxChange.emit({ event: event, options: option });
        this.autoCompleteElement.nativeElement.blur();
    }

    private subscribeToClosingActions(): any {
        if (this.subscription && !this.subscription.closed) {
            this.subscription.unsubscribe();
        }
        this.subscription = this.trigger.panelClosingActions
            .subscribe((e: any) => {
                let isValid: boolean = false;
                if (e && e.constructor.name === "MatOptionSelectionChange") {
                    isValid = true;

                }
                if (!isValid) {
                    this.formControl.setValue(null);
                    this.trigger.closePanel();
                }
            },
            err => this.subscribeToClosingActions(),
            () => this.subscribeToClosingActions());
    }

    private containsKey(val: any): any {
        let self: CommonAutoCompleteComponent = this;
        if (self.optionsData) {
            var displayValue: any = self.optionsData.filter((option: any) => option[self.optionKey] === val)[0];
            return displayValue;
        }
        return null;
    }

}

