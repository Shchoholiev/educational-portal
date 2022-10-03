import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MaterialBase } from 'src/app/shared/material-base.model';

@Component({
  selector: 'app-material',
  templateUrl: './material.component.html',
  styleUrls: ['./material.component.css']
})
export class MaterialComponent implements OnInit {

  @Input() index: number = 0;

  @Input() material: MaterialBase = new MaterialBase();

  @Output() remove = new EventEmitter<any>(true);

  constructor() { }

  ngOnInit(): void {
  }

}
