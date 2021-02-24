import { Injectable } from '@angular/core';
import { 
  HttpRequest,
  HttpHandler, 
  HttpEvent, 
  HttpInterceptor 
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { JwtService } from '../login';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
    constructor(private jwtService: JwtService, private router: Router) { console.log('ErrorInterceptor'); }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(request).pipe(catchError(err => {
            if (err.status === 401) {                
                this.jwtService.Logout();
                this.router.navigate(['login']);
                return throwError('401 redirect');
            }
            const error = err.error.message || err.statusText;
            return throwError(error);
        }))
    }
}