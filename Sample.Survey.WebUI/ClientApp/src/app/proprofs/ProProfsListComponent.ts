import { DatePipe } from '@angular/common';
import { ChangeDetectorRef, Component, Inject, LOCALE_ID, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { MatDialogRef } from '@angular/material';
import { map } from 'rxjs/operators';
import { ActivatedRoute, Router } from '@angular/router';
import { FxSamplesButton, FxSamplesDialog } from '../common/FxSamplesControlModels';
import { FxSamplesDialogService } from '../common/FxSamplesDialogService';
import { CommonService } from '@agility/frameworkcore';
import { ProProfsService } from './ProProfsService';
import { ProProfsUser } from './models/ProProfsUser';
import { HttpEvent } from '@angular/common/http';

@Component({
  selector: 'app-proprofs-list',
  templateUrl: './ProProfsListComponent.html',
})
export class ProProfsListComponent implements OnInit {
  public Payload: any;
  public dialogRef: MatDialogRef<any>;
  public dataList: any[] = [];
  public columns: any[];
  @ViewChild('editButton') editTmpl: TemplateRef<ProProfsListComponent>;
  public ProProfsModel: ProProfsUser;
  public FxSamplesDialog: FxSamplesDialog = new FxSamplesDialog();

  constructor(private service: ProProfsService, private route: ActivatedRoute,
    private dialogService: FxSamplesDialogService,
    private commonService: CommonService, private router: Router,
    private changedecref: ChangeDetectorRef) { }
  public Refresh() {
    if (this.dataList.length <= 0) {
      this.service.list().subscribe((response) => {
        this.ProProfsModel = response;
      });
    }
  }
  public ngOnInit(): void {
    this.route.data.pipe(map((response: HttpEvent<any>) => this._initialize(response))).subscribe((res) => {
      this.ProProfsModel = res['user'];
    });
    this.columns = [
      { name: 'Id', prop: 'quizId' },
      { name: 'Name', prop: 'quizUniqueName' },
      { name: 'Title', prop: 'quizTitle' },
      { name: 'Status', prop: 'quizStatus' },
      {
        cellTemplate: this.editTmpl, name: 'Actions', prop: 'Actions',
      },
    ];
  }
  public add(): void {
    this.router.navigate(['/ProProfs/New']);
  }
  _initialize(response: HttpEvent<any>): any {
    return response;
  }
}

