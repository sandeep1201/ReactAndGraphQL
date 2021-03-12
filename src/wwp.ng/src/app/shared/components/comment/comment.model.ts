import { Serializable } from '../../interfaces/serializable';
import { Utilities } from '../../utilities';
import { ValidationManager, ValidationResult, ValidationCode } from '../../models/validation';
import { CommentType } from './comment-type.model';

export class CommentModel implements Serializable<CommentModel> {
  id: number;
  commentText: string;
  commentTypeIds: number[];
  commentTypes: CommentType[];
  isEdited: boolean;
  createdDate: string;
  rowVersion: string;
  modifiedBy: string;
  modifiedDate: string;
  wiuid: string;

  public static create() {
    const comment = new CommentModel();
    comment.id = 0;
    return comment;
  }

  public static clone(input: any, instance: CommentModel) {
    instance.id = input.id;
    instance.commentText = input.commentText;
    instance.commentTypeIds = Utilities.deserilizeArray(input.commentTypeIds);
    instance.commentTypes = Utilities.deserilizeChildren(input.commentTypes, CommentType, 0);
    instance.isEdited = input.isEdited;
    instance.createdDate = input.createdDate;
    instance.rowVersion = input.rowVersion;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
    instance.wiuid = input.wiuid;
  }

  public deserialize(input: any) {
    CommentModel.clone(input, this);
    return this;
  }

  public validate(validationManager: ValidationManager): ValidationResult {
    const result = new ValidationResult();
    if (this.commentText == null || this.commentText.toString() === '') {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Comment');
      result.addError('commentText');
    }

    if (this.commentTypeIds == null || (this.commentTypeIds && this.commentTypeIds.length === 0)) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Comment Type');
      result.addError('commentTypes');
    }

    return result;
  }
}
