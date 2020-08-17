import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

import { Category } from '../models';
import { CategoryService } from '../services/category.service';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.css']
})
export class CategoriesComponent implements OnInit {
  categories: Category[] = [];

  form = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(50)]],
    type: ['Expense', Validators.required]
  });

  constructor(private fb: FormBuilder, private categoryService: CategoryService) { }

  get income(): Category[] {
    return this.categories.filter(c => c.type === 'Income');
  }

  get expenses(): Category[] {
    return this.categories.filter(c => c.type === 'Expense');
  }

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.categoryService.getAll().subscribe(categories => this.categories = categories);
  }

  add(): void {
    if (this.form.invalid) {
      return;
    }
    const type = this.form.value.type;
    this.categoryService.create(this.form.value).subscribe(() => {
      this.form.reset({ name: '', type });
      this.load();
    });
  }

  remove(category: Category): void {
    if (!confirm(`Delete "${category.name}"?`)) {
      return;
    }
    this.categoryService.delete(category.id).subscribe(() => this.load());
  }
}
