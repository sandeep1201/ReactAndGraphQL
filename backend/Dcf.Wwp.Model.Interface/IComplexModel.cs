using System;

namespace Dcf.Wwp.Model.Interface
{
    /// <summary>
    /// IComplexModel is used to indicate a model has properties that are not simple
    /// (i.e. int, string, etc.).  Rather they contain other object properties.  This
    /// enables a comparison at a top level object to be done in a generic fashion,
    /// specifically in the Repository.SaveIfChanged method.
    /// </summary>
    public interface IComplexModel
    {
        void SetModifiedOnComplexProperties<T>(T cloned, String user, DateTime modDate) where T : class, ICloneable, ICommonModel;
    }
}