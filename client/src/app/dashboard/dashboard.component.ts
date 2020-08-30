import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import * as Chart from 'chart.js';

import { MonthlyTotal } from '../models';
import { SummaryService } from '../services/summary.service';

const MONTH_LABELS = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  @ViewChild('monthlyCanvas', { static: true }) monthlyCanvas: ElementRef<HTMLCanvasElement>;

  year = new Date().getFullYear();
  totals: MonthlyTotal[] = [];

  private monthlyChart: Chart;

  constructor(private summaryService: SummaryService) { }

  ngOnInit(): void {
    this.summaryService.monthly(this.year).subscribe(totals => {
      this.totals = totals;
      this.renderMonthly();
    });
  }

  private renderMonthly(): void {
    if (this.monthlyChart) {
      this.monthlyChart.destroy();
    }

    this.monthlyChart = new Chart(this.monthlyCanvas.nativeElement, {
      type: 'bar',
      data: {
        labels: MONTH_LABELS,
        datasets: [
          { label: 'Income', data: this.totals.map(t => t.income), backgroundColor: '#59a14f' },
          { label: 'Expenses', data: this.totals.map(t => t.expense), backgroundColor: '#e15759' }
        ]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        scales: {
          yAxes: [{ ticks: { beginAtZero: true } }]
        }
      }
    });
  }
}
