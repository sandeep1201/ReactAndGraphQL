

export class Phone {
    public id: number;
    public type: string;
    public phoneNumber: string;
    public canText: boolean;
    public canVoiceMail: boolean;

    public static clone(input: any, instance: Phone) {
        instance.id           =  input.id;
        instance.type         =  input.type;
        instance.phoneNumber  =  input.phoneNumber;
        instance.canText      =  input.canText;
        instance.canVoiceMail =  input.canVoiceMail;

    }

    public deserialize(input: any): Phone {
        Phone.clone(input, this);
        return this;
    }
}
