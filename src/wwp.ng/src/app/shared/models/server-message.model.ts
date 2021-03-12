import { Serializable } from '../interfaces/serializable';
import { Utilities } from '../utilities';

export class RfaEligibility implements Serializable<RfaEligibility> {
    isEligible: boolean;
    serverMessages: ServerMessage[];

    public static clone(input: any, instance: RfaEligibility) {
        instance.isEligible = input.isEligible;
        instance.serverMessages = Utilities.deserilizeChildren(input.serverMessages, ServerMessage, 0) ;
    }

    public deserialize(input: any) {
        RfaEligibility.clone(input, this);
        return this;
    }
}


export class ServerMessage {
    code: string;
    message: string;

    public static clone(input: any, instance: ServerMessage) {
        instance.code = input.code;
        instance.message = input.message;
    }

    public deserialize(input: any) {
        ServerMessage.clone(input, this);
        return this;
    }
}