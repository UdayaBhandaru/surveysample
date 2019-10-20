var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Component, Input, Output, EventEmitter, ChangeDetectorRef } from "@angular/core";
import { FormGroup, FormControl } from "@angular/forms";
import "rxjs/add/operator/startWith";
var AutoCompleteComponentWithChips = /** @class */ (function () {
    function AutoCompleteComponentWithChips(ref) {
        this.ref = ref;
        this.change = new EventEmitter();
        this.chipSelected = new EventEmitter();
        this.chipOrientationVertical = "vertical";
        this.chipOrientationVerticalStyle = "mat-chip-list-stacked";
    }
    AutoCompleteComponentWithChips.prototype.ngOnInit = function () {
        var _self = this;
        _self._filteredOptions = _self.formControl.valueChanges
            .startWith(null)
            .map(function (x) { return x ? _self._filter(x) : _self.optionsData.slice(); });
        if (this.chipOrientationVertical == this.chipOrientation) {
            this.chipClass = this.chipOrientationVerticalStyle;
        }
        else {
            this.chipClass = "";
        }
    };
    AutoCompleteComponentWithChips.prototype._filter = function (val) {
        var _self = this;
        return val ? _self.optionsData.filter(function (option) { return new RegExp("^" + val, 'gi').test(option[_self.optionValue]); }) : _self.optionsData;
    };
    AutoCompleteComponentWithChips.prototype._displayFn = function () {
        var _self = this;
        return function (val) { return val ? val[_self.optionValue] : val; };
    };
    AutoCompleteComponentWithChips.prototype.onOptionSelect = function (event, option) {
        var _this = this;
        var _self = this;
        if (this.selectedChips.findIndex(function (x) { return x[_this.optionKey] == option[_this.optionKey]; }) == -1) {
            this.selectedChips.push(option);
            _self.change.emit({ event: event, options: option });
            _self.emitChipOptions(event, option);
        }
    };
    AutoCompleteComponentWithChips.prototype.ngAfterViewChecked = function () {
        var _self = this;
        this.ref.detectChanges();
        if (!_self.formControl.value) {
            var _self = this;
            _self._filteredOptions = _self.formControl.valueChanges
                .startWith(null)
                .map(function (x) { return x ? _self._filter(x) : _self.optionsData.slice(); });
        }
    };
    AutoCompleteComponentWithChips.prototype.remove = function (option) {
        var index = this.selectedChips.indexOf(option);
        var _self = this;
        if (index >= 0) {
            this.selectedChips.splice(index, 1);
            _self.emitChipOptions(event, option);
        }
    };
    AutoCompleteComponentWithChips.prototype.emitChipOptions = function (event, option) {
        var _self = this;
        _self.chipSelected.emit({ event: event, options: option, selectedChips: this.selectedChips });
    };
    __decorate([
        Input(),
        __metadata("design:type", FormGroup)
    ], AutoCompleteComponentWithChips.prototype, "formGroup", void 0);
    __decorate([
        Input(),
        __metadata("design:type", FormControl)
    ], AutoCompleteComponentWithChips.prototype, "formControl", void 0);
    __decorate([
        Input(),
        __metadata("design:type", String)
    ], AutoCompleteComponentWithChips.prototype, "placeholder", void 0);
    __decorate([
        Input(),
        __metadata("design:type", Object)
    ], AutoCompleteComponentWithChips.prototype, "optionsData", void 0);
    __decorate([
        Input(),
        __metadata("design:type", Object)
    ], AutoCompleteComponentWithChips.prototype, "optionKey", void 0);
    __decorate([
        Input(),
        __metadata("design:type", Object)
    ], AutoCompleteComponentWithChips.prototype, "optionValue", void 0);
    __decorate([
        Input(),
        __metadata("design:type", String)
    ], AutoCompleteComponentWithChips.prototype, "cssClass", void 0);
    __decorate([
        Input(),
        __metadata("design:type", Boolean)
    ], AutoCompleteComponentWithChips.prototype, "removableChip", void 0);
    __decorate([
        Input(),
        __metadata("design:type", String)
    ], AutoCompleteComponentWithChips.prototype, "chipOrientation", void 0);
    __decorate([
        Input(),
        __metadata("design:type", Object)
    ], AutoCompleteComponentWithChips.prototype, "selectedChips", void 0);
    __decorate([
        Input(),
        __metadata("design:type", Object)
    ], AutoCompleteComponentWithChips.prototype, "imageSrc", void 0);
    __decorate([
        Input(),
        __metadata("design:type", Object)
    ], AutoCompleteComponentWithChips.prototype, "imageClass", void 0);
    __decorate([
        Output(),
        __metadata("design:type", Object)
    ], AutoCompleteComponentWithChips.prototype, "change", void 0);
    __decorate([
        Output(),
        __metadata("design:type", Object)
    ], AutoCompleteComponentWithChips.prototype, "chipSelected", void 0);
    AutoCompleteComponentWithChips = __decorate([
        Component({
            selector: "autocomplete-chips",
            template: require("./AutoCompleteComponentWithChips.html")
        }),
        __metadata("design:paramtypes", [ChangeDetectorRef])
    ], AutoCompleteComponentWithChips);
    return AutoCompleteComponentWithChips;
}());
export { AutoCompleteComponentWithChips };
//# sourceMappingURL=AutoCompleteComponentWithChips.js.map