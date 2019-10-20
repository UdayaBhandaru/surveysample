var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Idle, DEFAULT_INTERRUPTSOURCES } from "@ng-idle/core";
import { Keepalive } from "@ng-idle/keepalive";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { FxDialogService, MessageResult, FxButton } from "@agility/framework";
var KeepAliveSessionService = /** @class */ (function () {
    function KeepAliveSessionService(router, keepalive, idle, _dialogService) {
        this.router = router;
        this.keepalive = keepalive;
        this.idle = idle;
        this._dialogService = _dialogService;
        this.sessionTimeOutMessage = "Your session is going to expire in {countdown} seconds !" + ",Would you like to extend the session?";
        this.messageResult = new MessageResult();
    }
    KeepAliveSessionService.prototype.setKeepAliveSessionConfiguration = function () {
        var _this = this;
        var timeInterval = 300;
        var idleTime = 1200;
        var idleTimeOut = 20;
        var idleTimeOutMilliSeconds = idleTimeOut * 1000;
        this.keepalive.request("/api/Home/KeepAliveSession");
        this.idle.setIdle(idleTime);
        this.idle.setTimeout(idleTimeOut);
        this.idle.setInterrupts(DEFAULT_INTERRUPTSOURCES);
        this.idle.onTimeout.subscribe(function () {
            _this.stop();
            _this.router.navigate(["/Account"]);
        });
        this.idle.onTimeoutWarning.subscribe(function (countdown) {
            _this.sessionTimeOutMessage = _this.sessionTimeOutMessage.replace("{countdown}", countdown);
            _this.validateAlert(_this.sessionTimeOutMessage, idleTimeOutMilliSeconds);
        });
        this.keepalive.interval(timeInterval);
    };
    KeepAliveSessionService.prototype.start = function () {
        this.keepalive.start();
        this.idle.watch();
    };
    KeepAliveSessionService.prototype.stop = function () {
        this.idle.stop();
        this.keepalive.stop();
    };
    KeepAliveSessionService.prototype.validateAlert = function (message, idleTimeOutMilliSeconds) {
        var _this = this;
        var setTimeOutHandle;
        this.stop();
        this.messageResult.message = this.sessionTimeOutMessage;
        var dialogRef = this._dialogService.openMessageDialog("Session Expiring", this.messageResult, [new FxButton("", "No", "alertCancel"),
            new FxButton("", "Yes", "alertdontsave")], "30%", "", "iconsdontsave retailsNavIcons", "dontsaveAlert", "dontsaveAlertTitle");
        dialogRef.componentInstance.click.subscribe(function (btnName) {
            if (btnName === "No") {
                _this.noClickEvent(setTimeOutHandle);
            }
            if (btnName === "Yes") {
                _this.yesClickEvent(setTimeOutHandle);
                _this.clearTimeout(setTimeOutHandle);
            }
        });
        setTimeOutHandle = setTimeout(function () {
            dialogRef.close();
            _this.noClickEvent(setTimeOutHandle);
        }, idleTimeOutMilliSeconds);
    };
    KeepAliveSessionService.prototype.clearTimeout = function (timeOutHandle) {
        window.clearTimeout(timeOutHandle);
    };
    KeepAliveSessionService.prototype.yesClickEvent = function (timeOutHandle) {
        this.clearTimeout(timeOutHandle);
        this.start();
    };
    KeepAliveSessionService.prototype.noClickEvent = function (timeOutHandle) {
        this.clearTimeout(timeOutHandle);
        this.stop();
        this.router.navigate(["/Account"]);
    };
    KeepAliveSessionService.prototype.ngOnDestroy = function () {
        if (this.idle) {
            this.stop();
        }
    };
    KeepAliveSessionService = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [Router, Keepalive, Idle, FxDialogService])
    ], KeepAliveSessionService);
    return KeepAliveSessionService;
}());
export { KeepAliveSessionService };
//# sourceMappingURL=KeepAliveSessionService.js.map