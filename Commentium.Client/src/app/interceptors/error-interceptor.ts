import { HttpInterceptorFn } from "@angular/common/http";
import { inject } from "@angular/core";
import { ToastrService } from "ngx-toastr";
import { catchError, throwError } from "rxjs";

export const errorInterceptorFn: HttpInterceptorFn = (req, next) => {
    const toastr = inject(ToastrService);
    
    return next(req).pipe(
      catchError((error) => {
        if(error.status >= 400 && error.status < 500){
          const problemDetails = error.error;
          toastr.warning(problemDetails.errors?.[0]?.description, `${problemDetails.errors?.[0]?.code}`);
        } else if (error.status >= 500) {
          toastr.error('An unexpected error occurred', `Error ${error.status}`);
        }
        return throwError(() => error);
      })
    );
  };