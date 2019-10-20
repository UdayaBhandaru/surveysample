import { Component, Input, OnInit, Output, EventEmitter, ViewChild } from "@angular/core";

@Component({

    selector: "fx-file-upload-new",
    template: `<input type="file" #{{controlname}} (change) = "getFiles($event)" [accept] = "acceptTypes" [multiple]="isMultiple">`
})
export class FxFileUpload implements OnInit {
    readonly invalidFileTypeMessage: string = "Please select a valid file type,The accepted file types are";
    fileString: string;
    fileName: string;
    @Input() acceptTypes: string;
    @Input() isMultiple: boolean;
    @Output() fileChange = new EventEmitter();
    @Output() uploadedFileReference = new EventEmitter();
    @Input() controlname: string;
    @ViewChild("{{controlname}}")
    uploadedFile: any;

    ngOnInit(): void {
        if (this.uploadedFileReference.observers.length > 0) {
            this.uploadedFileReference.emit({ "fileReference": this.uploadedFile });
        }
    }

    ngOnDestroy(): void {
        if (this.fileChange.observers.length > 0) {
            this.fileChange.unsubscribe();
            this.fileChange = null;
        }

        if (this.uploadedFileReference.observers.length > 0) {
            this.uploadedFileReference.unsubscribe();
            this.uploadedFileReference = null;
        }
    }

    public getFiles(event: any): void {
        let files: FileList;
        this.resetFileObjects();
        files = event.target.files;
        for (var myfile: number = 0; myfile <= files.length - 1; myfile++) {
            if (this.validateFileTypes(files[myfile])) {
                var reader: FileReader = new FileReader();
                this.fileName = files[myfile].name;
                reader.readAsDataURL(files[myfile]);
                reader.onloadend = (e) => {
                  // this.fileString = reader.result.split("base64,")[1];
                  this.fileString = (reader.result as string).split("base64,")[1];
                    if (this.fileChange.observers.length > 0) {
                        this.fileChange.emit({ "filename": this.fileName, "filedataasbase64": this.fileString, "filedata": reader.result });
                        reader = null;
                    }
                };
            }
        }
    }

    private resetFileObjects(): void {
        this.fileString = "";
        this.fileName = "";
    }

    private validateFileTypes(file: File): boolean {
        if (this.acceptTypes) {
            var splitFile: string[] = file.name.split(".");
            let fileType: string = "." + splitFile[splitFile.length - 1];
            if (this.acceptTypes.toUpperCase().indexOf(fileType.toUpperCase()) === -1) {
                if (this.fileChange.observers.length > 0) {
                    this.fileChange.emit({ "ErrorMessage": this.invalidFileTypeMessage.concat(this.acceptTypes) });
                }
                this.uploadedFile.nativeElement.value = "";
                return false;
            }
        }
        return true;

    }
}
