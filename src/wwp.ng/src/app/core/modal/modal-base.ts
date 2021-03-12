export class ModalBase {

    public destroy: Function = () => { };
    public closeDialog: Function = () => { };
    exit() {
        this.closeDialog();
        this.destroy();
    }
}
