import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Skill } from 'src/app/shared/skill.model';

@Component({
  selector: 'app-skill',
  templateUrl: './skill.component.html',
  styleUrls: ['./skill.component.css']
})
export class SkillComponent implements OnInit {

  @Input() skill: Skill = new Skill();

  @Output() remove = new EventEmitter<any>(true);

  constructor() { }

  ngOnInit(): void {
  }
}
