import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import * as Chart from 'chart.js';

import { CategoryTotal, MonthlyTotal } from '../models';
import { SummaryService } from '../services/summary.service';

const MONTH_LABELS = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  @ViewChild('monthlyCanvas', { static: true }) monthlyCanvas: ElementRef<HTMLCanvasElement>;
  @ViewChild('breakdownCanvas', { static: true }) breakdownCanvas: ElementRef<HTMLCanvasElement>;

  year = new Date().getFullYear();
  month = new Date().getMonth() + 1;
  monthLabels = MONTH_LABELS;
  totals: MonthlyTotal[] = [];
  breakdown: CategoryTotal[] = [];

  private monthlyChart: Chart;
  private breakdownChart: Chart;

  constructor(private summaryService: SummaryService) { }

  ngOnInit(): void {
    this.summaryService.monthly(this.year).subscribe(totals => {
      this.totals = totals;
      this.renderMonthly();
    });
    this.loadBreakdown();
  }

  selectMonth(month: number): void {
    this.month = month;
    this.loadBreakdown();
  }

  private loadBreakdown(): void {
    this.summaryService.byCategory(this.year, this.month).subscribe(breakdown => {
      this.breakdown = breakdown;
      this.renderBreakdown();
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
        onClick: (event, elements) => {
          if (elements && elements.length) {
            // tslint:disable-next-line:no-any
            this.selectMonth((elements[0] as any)._index + 1);
          }
        },
        scales: {
          yAxes: [{ ticks: { beginAtZero: true } }]
        }
      }
    });
  }

  private renderBreakdown(): void {
    if (this.breakdownChart) {
      this.breakdownChart.destroy();
    }

    this.breakdownChart = new Chart(this.breakdownCanvas.nativeElement, {
      type: 'doughnut',
      data: {
        labels: this.breakdown.map(b => b.name),
        datasets: [{
          data: this.breakdown.map(b => b.total),
          backgroundColor: this.breakdown.map(b => b.color || '#bab0ac')
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        legend: { position: 'right' }
      }
    });
  }
}
