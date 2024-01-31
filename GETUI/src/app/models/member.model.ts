export class MemberModel {
    public  id!:number;
    public userName!:string;
    public memberFullName!:string;
    public planType!:string;
    public joiningDate!:Date;
  }

 export class TeamTree {
  public  id!:number;
  public name!:string;
  public treeOrder!:number;
  public rate!:number;
  public package!:string;
  public count!:number;
  public fullName!:string;

} 

export class TeamTreeModel {
  public level?: string;
  public count?: number;
  public teamIncome?:number;
}