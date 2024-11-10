import { Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root'
})
export class CaptchaService {

    private captchaText: string = '';

    generateCaptchaString(length: number = 6): string {
        const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
        let result = '';
      
        for (let i = 0; i < length; i++) {
          const randomIndex = Math.floor(Math.random() * characters.length);
          result += characters[randomIndex];
        }
      
        this.captchaText = result;
        return result;
    }

    isValidCaptch(input: string): boolean {
        return input === this.captchaText;
    }
}