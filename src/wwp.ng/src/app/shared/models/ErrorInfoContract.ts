import { ValidationError } from './validation';
export class ErrorInfoContract {
    public targetUrl: string;

    public success: boolean;

    public error: ErrorInfo;

    public unAuthorizedRequest: boolean;

    public __apiWrapped: boolean = false;

}

export class GenericErrorInfoContract<T> extends ErrorInfoContract {
    public result: T;
}

export class ErrorInfo {
    public code: number = 0;
    public message: string;
    public details: string;
    public validationErrors: ValidationErrorInfo[] = [];
    public content: any;

    public static deserialize(contract: ErrorInfoContract): ErrorInfo {
        const errorInfo = new ErrorInfo();
        errorInfo.code = contract.error.code;
        errorInfo.content = contract.error.content;
        errorInfo.details = contract.error.details;
        errorInfo.message = contract.error.message;
        errorInfo.validationErrors = contract.error.validationErrors;
        return errorInfo;
    }
}

export class ValidationErrorInfo {
    public message: String;
    public members: string[] = [];
}