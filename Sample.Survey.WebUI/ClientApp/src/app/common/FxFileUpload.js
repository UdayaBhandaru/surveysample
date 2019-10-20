var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Component, Input, Output, EventEmitter, ViewChild } from "@angular/core";
var FxFileUpload = /** @class */ (function () {
    function FxFileUpload() {
        this.invalidFileTypeMessage = "Please select a valid file type,The accepted file types are";
        this.fileChange = new EventEmitter();
        this.uploadedFileReference = new EventEmitter();
    }
    FxFileUpload.prototype.ngOnInit = function () {
        if (this.uploadedFileReference.observers.length > 0) {
            this.uploadedFileReference.emit({ "fileReference": this.uploadedFile });
        }
    };
    FxFileUpload.prototype.ngOnDestroy = function () {
        if (this.fileChange.observers.length > 0) {
            this.fileChange.unsubscribe();
            this.fileChange = null;
        }
        if (this.uploadedFileReference.observers.length > 0) {
            this.uploadedFileReference.unsubscribe();
            this.uploadedFileReference = null;
        }
    };
    FxFileUpload.prototype.getFiles = function (event) {
        var _this = this;
        var files;
        this.resetFileObjects();
        files = event.target.files;
        for (var myfile = 0; myfile <= files.length - 1; myfile++) {
            if (this.validateFileTypes(files[myfile])) {
                var reader = new FileReader();
                this.fileName = files[myfile].name;
                reader.readAsDataURL(files[myfile]);
                reader.onloadend = function (e) {
                    _this.fileString = reader.result.split("base64,")[1];
                    if (_this.fileChange.observers.length > 0) {
                        _this.fileChange.emit({ "filename": _this.fileName, "filedataasbase64": _this.fileString, "filedata": reader.result });
                        reader = null;
                    }
                };
            }
        }
    };
    FxFileUpload.prototype.resetFileObjects = function () {
        this.fileString = "";
        this.fileName = "";
    };
    FxFileUpload.prototype.validateFileTypes = function (file) {
        if (this.acceptTypes) {
            var splitFile = file.name.split(".");
            var fileType = "." + splitFile[splitFile.length - 1];
            if (this.acceptTypes.toUpperCase().indexOf(fileType.toUpperCase()) === -1) {
                if (this.fileChange.observers.length > 0) {
                    this.fileChange.emit({ "ErrorMessage": this.invalidFileTypeMessage.concat(this.acceptTypes) });
                }
                this.uploadedFile.nativeElement.value = "";
                return false;
            }
        }
        return true;
    };
    __decorate([
        Input(),
        __metadata("design:type", String)
    ], FxFileUpload.prototype, "acceptTypes", void 0);
    __decorate([
        Input(),
        __metadata("design:type", Boolean)
    ], FxFileUpload.prototype, "isMultiple", void 0);
    __decorate([
        Output(),
        __metadata("design:type", Object)
    ], FxFileUpload.prototype, "fileChange", void 0);
    __decorate([
        Output(),
        __metadata("design:type", Object)
    ], FxFileUpload.prototype, "uploadedFileReference", void 0);
    __decorate([
        Input(),
        __metadata("design:type", String)
    ], FxFileUpload.prototype, "controlname", void 0);
    __decorate([
        ViewChild("{{controlname}}"),
        __metadata("design:type", Object)
    ], FxFileUpload.prototype, "uploadedFile", void 0);
    FxFileUpload = __decorate([
        Component({
            selector: "fx-file-upload-new",
            template: "<input type=\"file\" #{{controlname}} (change) = \"getFiles($event)\" [accept] = \"acceptTypes\" [multiple]=\"isMultiple\">"
        })
    ], FxFileUpload);
    return FxFileUpload;
}());
export { FxFileUpload };
//# sourceMappingURL=FxFileUpload.js.map