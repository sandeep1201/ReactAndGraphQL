import { Serializable } from '../../interfaces/serializable';
import { ValidationManager, ValidationResult } from '../../models/validation';


export class CommentType implements Serializable<CommentType> {
  commentTypeId: number;
  commentTypeName: string;
  isSystemUseOnly: boolean;

  public static clone(input: any, instance: CommentType) {
    instance.commentTypeId = input.commentTypeId;
    instance.commentTypeName = input.commentTypeName;
    instance.isSystemUseOnly = input.isSystemUseOnly;
  }

  public deserialize(input: any) {
    CommentType.clone(input, this);
    return this;
  }

  public validate(validationManager: ValidationManager, pinCommentType: CommentType): ValidationResult {
    const result = new ValidationResult();
    return result;
  }
}
