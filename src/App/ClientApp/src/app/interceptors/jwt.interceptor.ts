import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent
} from '@angular/common/http';

import { Observable } from 'rxjs';
import { JwtService } from '../login';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor(private jwtService: JwtService) { console.log('JwtInterceptor'); }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        if (request.url.includes('api/')) {
          const identity = this.jwtService.Identity;
          if(identity && identity.token)
          {
            request = request.clone({
                setHeaders: {
                    Authorization: `Bearer ${identity.token}`
                }
            });
          }
        }

        return next.handle(request);
    }
}