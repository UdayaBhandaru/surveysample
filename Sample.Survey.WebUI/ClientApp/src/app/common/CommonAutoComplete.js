var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Component, Input, Output, EventEmitter, ChangeDetectorRef, ViewChild } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { FormGroup, FormControl } from "@angular/forms";
import "rxjs/add/operator/startWith";
import "rxjs/add/operator/debounceTime";
import "rxjs/add/operator/map";
import { MatAutocompleteTrigger } from "@angular/material";
var CommonAutoCompleteComponent = /** @class */ (function () {
    function CommonAutoCompleteComponent(ref, http) {
        this.ref = ref;
        this.http = http;
        this.minLength = 0;
        this.asyncString = " | async ";
        this.fxChange = new EventEmitter();
    }
    CommonAutoCompleteComponent.prototype.ngOnInit = function () {
        var self = this;
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
        }
        else {
            self.filteredOptions = self.formControl.valueChanges
                .startWith(null)
                .map(function (x) { return x ? self.filter(x) : (self.optionsData ? self.optionsData.slice() : null); });
        }
    };
    CommonAutoCompleteComponent.prototype.ngAfterViewChecked = function () {
        var self = this;
        this.ref.detach();
        this.ref.detectChanges();
        if (!self.isDataFromApiOnValueChange) {
            if (!self.formControl.value) {
                self.filteredOptions = self.formControl.valueChanges
                    .startWith(null)
                    .map(function (x) { return x ? self.filter(x) : (self.optionsData ? self.optionsData.slice() : null); });
            }
        }
        if (self.formControl && self.disabled) {
            this.formControl.disable();
        }
    };
    CommonAutoCompleteComponent.prototype.ngAfterViewInit = function () {
        this.subscribeToClosingActions();
    };
    CommonAutoCompleteComponent.prototype.ngOnDestroy = function () {
        if (!this.subscription && !this.subscription.closed) {
            this.subscription.unsubscribe();
        }
    };
    CommonAutoCompleteComponent.prototype.setApiToAutoComplete = function () {
        var _this = this;
        var self = this;
        self.formControl.valueChanges.debounceTime(400).subscribe(function (word) {
            if (word && word.length >= _this.minLength) {
                _this.getDataFromAPI(word).subscribe(function (response) {
                    self.filteredOptions = response;
                    self.optionsData = self.filteredOptions;
                });
            }
            else {
                self.filteredOptions = null;
            }
        });
    };
    CommonAutoCompleteComponent.prototype.getDataFromAPI = function (word) {
        var resp = this.http.get(this.apiUrl + word);
        //var resp: any = this.http.get(this.apiUrl + word).map((resp: Response) => {
        //    return resp.json();
        //});
        return resp;
    };
    CommonAutoCompleteComponent.prototype.filter = function (val) {
        var self = this;
        return val ? self.optionsData.filter(function (option) { return new RegExp("^" + val, "gi").test(option[self.optionValue]); }) : self.optionsData;
    };
    CommonAutoCompleteComponent.prototype.displayFn = function (value) {
        return value ? this.getDisplayValue(value) : value;
    };
    CommonAutoCompleteComponent.prototype.getDisplayValue = function (val) {
        var self = this;
        if (self.optionsData) {
            var displayValue = self.optionsData.filter(function (option) { return option[self.optionKey] === val; })[0][self.optionValue];
            return displayValue;
        }
        return null;
    };
    CommonAutoCompleteComponent.prototype.onOptionSelect = function (event) {
        var _this = this;
        var self = this;
        var option = this.optionsData.find(function (x) { return x[_this.optionKey] === event.option.value; });
        self.fxChange.emit({ event: event, options: option });
        this.autoCompleteElement.nativeElement.blur();
    };
    CommonAutoCompleteComponent.prototype.subscribeToClosingActions = function () {
        var _this = this;
        if (this.subscription && !this.subscription.closed) {
            this.subscription.unsubscribe();
        }
        this.subscription = this.trigger.panelClosingActions
            .subscribe(function (e) {
            var isValid = false;
            if (e && e.constructor.name === "MatOptionSelectionChange") {
                isValid = true;
            }
            if (!isValid) {
                _this.formControl.setValue(null);
                _this.trigger.closePanel();
            }
        }, function (err) { return _this.subscribeToClosingActions(); }, function () { return _this.subscribeToClosingActions(); });
    };
    CommonAutoCompleteComponent.prototype.containsKey = function (val) {
        var self = this;
        if (self.optionsData) {
            var displayValue = self.optionsData.filter(function (option) { return option[self.optionKey] === val; })[0];
            return displayValue;
        }
        return null;
    };
    __decorate([
        Input(),
        __metadata("design:type", FormGroup)
    ], CommonAutoCompleteComponent.prototype, "formGroup", void 0);
    __decorate([
        Input(),
        __metadata("design:type", FormControl)
    ], CommonAutoCompleteComponent.prototype, "formControl", void 0);
    __decorate([
        Input(),
        __metadata("design:type", Boolean)
    ], CommonAutoCompleteComponent.prototype, "required", void 0);
    __decorate([
        Input(),
        __metadata("design:type", String)
    ], CommonAutoCompleteComponent.prototype, "placeholder", void 0);
    __decorate([
        Input(),
        __metadata("design:type", Object)
    ], CommonAutoCompleteComponent.prototype, "optionsData", void 0);
    __decorate([
        Input(),
        __metadata("design:type", Object)
    ], CommonAutoCompleteComponent.prototype, "optionKey", void 0);
    __decorate([
        Input(),
        __metadata("design:type", Object)
    ], CommonAutoCompleteComponent.prototype, "optionValue", void 0);
    __decorate([
        Input(),
        __metadata("design:type", String)
    ], CommonAutoCompleteComponent.prototype, "cssClass", void 0);
    __decorate([
        Input(),
        __metadata("design:type", Object)
    ], CommonAutoCompleteComponent.prototype, "imageSrc", void 0);
    __decorate([
        Input(),
        __metadata("design:type", Object)
    ], CommonAutoCompleteComponent.prototype, "imageClass", void 0);
    __decorate([
        Input(),
        __metadata("design:type", Number)
    ], CommonAutoCompleteComponent.prototype, "minLength", void 0);
    __decorate([
        Input(),
        __metadata("design:type", String)
    ], CommonAutoCompleteComponent.prototype, "apiUrl", void 0);
    __decorate([
        Input(),
        __metadata("design:type", Boolean)
    ], CommonAutoCompleteComponent.prototype, "disabled", void 0);
    __decorate([
        Output(),
        __metadata("design:type", Object)
    ], CommonAutoCompleteComponent.prototype, "fxChange", void 0);
    __decorate([
        ViewChild(MatAutocompleteTrigger),
        __metadata("design:type", MatAutocompleteTrigger)
    ], CommonAutoCompleteComponent.prototype, "trigger", void 0);
    __decorate([
        ViewChild("autoCompleteElement"),
        __metadata("design:type", Object)
    ], CommonAutoCompleteComponent.prototype, "autoCompleteElement", void 0);
    CommonAutoCompleteComponent = __decorate([
        Component({
            selector: "autocomplete",
            template: "<mat-form-field [formGroup]=\"formGroup\" class=\"{{cssClass}} material-auto-complete\">\n    <input matInput type=\"text\" placeholder=\"{{placeholder}}\"\n           [formControl]=\"formControl\" [matAutocomplete]=\"auto\" [required]=\"required\" #autoCompleteElement/>\n<div class=\"autocomplte-search-icon retailsNavIcons\"></div>\n</mat-form-field>\n<mat-autocomplete #auto=\"matAutocomplete\" [displayWith]=\"displayFn.bind(this)\" (optionSelected)=\"onOptionSelect($event)\">\n    <div *ngIf=\"isDataFromApiOnValueChange\">\n    <mat-option *ngFor=\"let option of filteredOptions\" [value]=\"option[optionKey]\" >\n     <span style='text-align: center'><img *ngIf='option[imageSrc]' [src]='option[imageSrc]'  [class]='imageClass'/></span>\n    <span  style='text-align: center'>{{option[optionValue]}}</span>\n    </mat-option>\n    </div>\n    <div *ngIf=\"!isDataFromApiOnValueChange\">\n    <mat-option *ngFor=\"let option of filteredOptions | async\" [value]=\"option[optionKey]\">\n     <span style='text-align: center'><img *ngIf='option[imageSrc]' [src]='option[imageSrc]'  [class]='imageClass'/></span>\n     <span  style='text-align: center'>{{option[optionValue]}}</span>\n    </mat-option>\n    </div>\n</mat-autocomplete>"
        }),
        __metadata("design:paramtypes", [ChangeDetectorRef, HttpClient])
    ], CommonAutoCompleteComponent);
    return CommonAutoCompleteComponent;
}());
export { CommonAutoCompleteComponent };
//# sourceMappingURL=CommonAutoComplete.js.map