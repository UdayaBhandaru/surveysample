import { NgModule } from "@angular/core";
import { FrameworkCoreModule } from "@agility/frameworkcore";

import { CommonAutoCompleteComponent } from "./CommonAutoComplete";
import { AutoCompleteComponentWithChips } from "./AutoCompleteComponentWithChips";
import { FxFileUpload} from "./FxFileUpload";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { FxSamplesDialogService } from "./FxSamplesDialogService";
import { NgxDatatableModule } from "@swimlane/ngx-datatable";
import { FxSamplesMessageDialogComponent } from "./FxSamplesDialogComponent";
import {
  MatFormFieldModule, MatTooltipModule, MatAutocompleteModule, MatButtonModule, MatCardModule, MatCheckboxModule,
  MatChipsModule, MatCommonModule, MatDatepickerModule, MatDialogModule, MatIconModule, MatInputModule, MatNativeDateModule,
  MatRadioModule, MatSelectModule, MatSidenavModule, MatSnackBarModule, MatTabsModule, MatTableModule, MatPaginatorModule
} from "@angular/material";
import { FlexLayoutModule } from "@angular/flex-layout";
import { FxPermissionDirective } from "./FxPermissionDirective";

@NgModule({
  imports: [FrameworkCoreModule, MatIconModule, MatFormFieldModule, MatAutocompleteModule,
    FormsModule, ReactiveFormsModule, MatFormFieldModule, MatTooltipModule, MatAutocompleteModule, MatButtonModule, MatCardModule, MatCheckboxModule,
    MatChipsModule, MatCommonModule, MatDatepickerModule, MatDialogModule, MatIconModule, MatInputModule, MatNativeDateModule,
    MatRadioModule, MatSelectModule, MatSidenavModule, MatSnackBarModule, MatTabsModule, MatTableModule, MatPaginatorModule,
    FlexLayoutModule, NgxDatatableModule],
    declarations: [CommonAutoCompleteComponent, AutoCompleteComponentWithChips, FxFileUpload, FxPermissionDirective,
FxSamplesMessageDialogComponent],
  exports: [CommonAutoCompleteComponent, AutoCompleteComponentWithChips, FxFileUpload, MatFormFieldModule, MatTooltipModule, MatAutocompleteModule, MatButtonModule, MatCardModule, MatCheckboxModule,
    MatChipsModule, MatCommonModule, MatDatepickerModule, MatDialogModule, MatIconModule, MatInputModule, MatNativeDateModule, FxPermissionDirective,
    MatRadioModule, MatSelectModule, MatSidenavModule, MatSnackBarModule, MatTabsModule, MatTableModule, MatPaginatorModule, FlexLayoutModule, NgxDatatableModule, ReactiveFormsModule,
    FxSamplesMessageDialogComponent],
  providers: [FxSamplesDialogService ]
   
})
export class CoreModule {
}
