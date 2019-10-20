import { Component, Input, OnInit, Output, EventEmitter, ChangeDetectorRef } from "@angular/core";
import { FormGroup, FormControl } from "@angular/forms";
import { Observable } from "rxjs";
import { startWith, map } from "rxjs/operators";

@Component({
    selector: "autocomplete-chips",
    templateUrl: "./AutoCompleteComponentWithChips.html"
})
export class AutoCompleteComponentWithChips {
    @Input() formGroup: FormGroup;
    @Input() formControl: FormControl;
    @Input() placeholder: string;
    @Input() optionsData: any;
    @Input() optionKey: any;
    @Input() optionValue: any;
    @Input() cssClass: string;
    @Input() removableChip: boolean;
    @Input() chipOrientation: string;
    @Input() selectedChips: any;
    @Input() imageSrc: any;
    @Input() imageClass: any;
    public _filteredOptions: Observable<any>;
    @Output() change = new EventEmitter();
    @Output() chipSelected = new EventEmitter();
    private _self: AutoCompleteComponentWithChips;
    public chipClass: string;
    private chipOrientationVertical = "vertical";
    private chipOrientationVerticalStyle = "mat-chip-list-stacked";
    constructor(private ref: ChangeDetectorRef) {

    }

    ngOnInit(): void {
        let _self = this;
      _self._filteredOptions = _self.formControl.valueChanges
        .pipe(startWith(null),
          map(x => x ? _self._filter(x) : _self.optionsData.slice()));
        if (this.chipOrientationVertical == this.chipOrientation) {
            this.chipClass = this.chipOrientationVerticalStyle;
        }
        else {
            this.chipClass = "";
        }
    }

  public _filter(val: string): any {
        let _self = this;
        return val ? _self.optionsData.filter((option: any) => new RegExp(`^${val}`, 'gi').test(option[_self.optionValue])) : _self.optionsData;
    }

    public _displayFn() {
        let _self = this;
        return (val: any) => val ? val[_self.optionValue] : val;
    }

    onOptionSelect(event: any, option: any) {
        let _self = this;
        if (this.selectedChips.findIndex(x => x[this.optionKey] == option[this.optionKey]) == -1) {
            this.selectedChips.push(option);
            _self.change.emit({ event: event, options: option });
            _self.emitChipOptions(event, option);
        }
    }

    ngAfterViewChecked(): void {
        var _self = this;
        this.ref.detectChanges();
        if (!_self.formControl.value) {
            var _self = this;
            _self._filteredOptions = _self.formControl.valueChanges
                .pipe(startWith(null),
                map(function (x) { return x ? _self._filter(x) : _self.optionsData.slice(); }));
        }
    }

    remove(option: any): void {
        let index = this.selectedChips.indexOf(option);
        var _self = this;
        if (index >= 0) {
            this.selectedChips.splice(index, 1);
            _self.emitChipOptions(event, option);
        }
    }

    emitChipOptions(event: any, option: any): void {
        var _self = this;
        _self.chipSelected.emit({ event: event, options: option, selectedChips: this.selectedChips });
    }
}
