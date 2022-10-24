import { Component, Input, OnInit } from '@angular/core';
import { SkillLookup } from 'src/app/shared/lookup-models/skill-lookup.model';
import { Skill } from 'src/app/shared/skill.model';
import { SkillsService } from 'src/app/skills/skills.service';

@Component({
  selector: 'app-choose-skills',
  templateUrl: './choose-skills.component.html',
  styleUrls: ['./choose-skills.component.css']
})
export class ChooseSkillsComponent implements OnInit {

  @Input() skillLookups: SkillLookup[] = [];

  public skills: Skill[] = [];

  public metadata: any;
  
  public pageSize = 6;

  public skill: Skill = new Skill();

  constructor(private _skillsService: SkillsService) { }

  ngOnInit(): void {
    this.setPage(1);
  }

  public chooseSkill(id: number){
    if (this.skillChosen(id)) {
      var index = this.skillLookups.findIndex(s => s.skillId == id);
      this.skillLookups.splice(index, 1);
    }
    else{
      var skill = this.skills.find(s => s.id == id);
      if (skill) {
        var skillLookup = new SkillLookup();
        skillLookup.skillId = skill.id,
        skillLookup.skillName = skill.name,
        skillLookup.level = 1;
        this.skillLookups.push(skillLookup);
      }
    }
  }

  public setPage(pageNumber: number){
    this._skillsService.getPage(this.pageSize, pageNumber)
    .subscribe(
      response => { 
        this.skills = response.body as Skill[];
        var metadata = response.headers.get('x-pagination');
        if (metadata) {
          this.metadata = JSON.parse(metadata);
        }
      }
    )
  }

  public skillChosen(skillId: number){
    return this.skillLookups.find(s => s.skillId == skillId);
  }
}
