﻿import { ErrorHandler, Inject, NgZone } from '@angular/core';
import { ToastyService } from "ng2-toasty";

export class AppErrorHandler implements ErrorHandler {
    constructor(
        private ngZone: NgZone,
        @Inject(ToastyService) private toastyService: ToastyService) {
    }

    handleError(error: any): void {
        //console.log("Error");

        //if (!isDevMode())
        //    Raven.captureException(error.orginalError || error);
        //else
        //    throw error;

        this.ngZone.run(() => {
            this.toastyService.error({
                title: 'Error',
                msg: 'An unexpected error happened.',
                theme: 'bootstrap',
                showClose: true,
                timeout: 5000
            });
        });        
    }
}