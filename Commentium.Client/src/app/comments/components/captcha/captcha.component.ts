import { Component, ElementRef, ViewChild, Output, EventEmitter, OnInit } from '@angular/core';
import { CaptchaService } from '../../services/captcha.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
    selector: 'captcha',
    standalone: true,
    imports: [CommonModule, FormsModule],
    templateUrl: './captcha.component.html'
})
export class CaptchaComponent implements OnInit {
  @ViewChild('captchaCanvas', { static: true }) captchaCanvas!: ElementRef<HTMLCanvasElement>;
  @Output() captchaValid = new EventEmitter<boolean>();

  captchaText: string = '';
  captchaInput: string = '';
  isCaptchaInvalid: boolean = false;

  constructor(private captchaService: CaptchaService) {}

  ngOnInit(): void {
    this.generateCaptcha();
  }

  generateCaptcha(): void {
    this.captchaText = this.captchaService.generateCaptchaString();
    const canvas = this.captchaCanvas.nativeElement;
    const context = canvas.getContext('2d');

    if (context) {
      context.clearRect(0, 0, canvas.width, canvas.height);

      context.font = '50px Arial';
      context.fillStyle = '#000';

      const textWidth = context.measureText(this.captchaText).width;
      const textHeight = 30;

      const x = (canvas.width - textWidth) / 2;
      const y = (canvas.height + textHeight) / 2;

      context.fillText(this.captchaText, x, y);

      context.strokeStyle = 'black';
      context.lineWidth = 2;
      context.beginPath();

      const strikeThroughY = y - textHeight / 2;
      context.moveTo(x, strikeThroughY);
      context.lineTo(x + textWidth, strikeThroughY);

      context.stroke();
    }
  }

  validateCaptcha(): void {
    this.isCaptchaInvalid = !this.captchaService.isValidCaptch(this.captchaInput);
    this.captchaValid.emit(!this.isCaptchaInvalid);
  }

  resetCaptcha(): void {
    this.captchaInput = '';
    this.isCaptchaInvalid = false;
    this.generateCaptcha();
  }
}