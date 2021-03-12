using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IBarrierDetail : ICommonDelModel, ICloneable
    {
        Int32?                                        ParticipantId                              { get; set; }
        Int32?                                        BarrierTypeId                              { get; set; }
        Int32?                                        BarrierSectionId                           { get; set; }
        DateTime?                                     OnsetDate                                  { get; set; }
        DateTime?                                     EndDate                                    { get; set; }
        Boolean?                                      IsAccommodationNeeded                      { get; set; }
        Boolean                                       WasClosedAtDisenrollment                   { get; set; }
        String                                        Details                                    { get; set; }
        Boolean                                       IsOpen                                     { get; }
        bool?                                         IsConverted                                { get; set; }
        ICollection<IBarrierAccommodation>            BarrierAccommodations                      { get; set; }
        ICollection<IBarrierAccommodation>            NonDeletedBarrierAccommodations            { get; }
        IBarrierSection                               BarrierSection                             { get; set; }
        ICollection<IBarrierTypeBarrierSubTypeBridge> BarrierTypeBarrierSubTypeBridges           { get; set; }
        ICollection<IBarrierTypeBarrierSubTypeBridge> NonDeletedBarrierTypeBarrierSubTypeBridges { get; }
        IBarrierType                                  BarrierType                                { get; set; }
        IParticipant                                  Participant                                { get; set; }
        ICollection<IBarrierDetailContactBridge>      BarrierDetailContactBridges                { get; set; }
        ICollection<IBarrierDetailContactBridge>      NonBarrierDetailContactBridges             { get; }
        ICollection<IFormalAssessment>                FormalAssessments                          { get; set; }
        ICollection<IFormalAssessment>                NonDeletedFormalAssessments                { get; }
    }
}
