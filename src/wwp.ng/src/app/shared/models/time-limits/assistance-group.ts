import { AssistanceGroupMember } from './assistance-group-member'
import { AssistanceGroupContract } from '../../services/contracts/timelimits/service.contract';
export class AssistanceGroup {
    parents: AssistanceGroupMember[] = [];
    children: AssistanceGroupMember[] = [];

    public clone(){
        let newGroup = new AssistanceGroup();
        this.parents.map(x=>{
            newGroup.parents.push(x.clone())
        });
        this.children.map(x=>{
            newGroup.children.push(x.clone())
        })
        return newGroup;
    }

    public deserialize(contract:AssistanceGroupContract){
        for(let memberContract of contract.parents){
            let parent = new AssistanceGroupMember();
            parent.deserialize(memberContract);
            this.parents.push(parent);
        }
        for(let memberContract of contract.children){
            let child = new AssistanceGroupMember();
            child.deserialize(memberContract);
            this.children.push(child);
        }
    }
}
