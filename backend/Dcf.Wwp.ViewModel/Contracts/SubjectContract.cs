using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Dcf.Wwp.Api.Library.Contracts
{
    [DataContract]
    public class SubjectContract
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "subjectId")]
        public int SubjectTypeId { get; set; }

        [DataMember(Name = "score")]
        public int? Score { get; set; }

        [DataMember(Name = "level")]
        public string Level { get; set; }

        [DataMember(Name = "form")]
        public string Form { get; set; }

        [DataMember(Name = "casasGradeEquivalency")]
        public string CasasGradeEquivalency { get; set; }

        [DataMember(Name = "nrsId")]
        public int? NrsTypeId { get; set; }

        [DataMember(Name = "nrsRating")]
        public string NrsTypeRating { get; set; }

        [DataMember(Name = "splId")]
        public int? SplTypeId { get; set; }

        [DataMember(Name = "splRating")]
        public string SplTypeRating { get; set; }

        //[DataMember(Name = "examLevelType")]
        //public int? ExamLevelType { get; set; }

        [DataMember(Name = "hasPassed")]
        public int? ExamPassTypeId { get; set; }

        [DataMember(Name = "gradeEquivalency")]
        public decimal? GradeEquivalency { get; set; }

        [DataMember(Name = "datePassed")]
        public string DatePassed { get; set; }

        //[DataMember(Name = "versionTypeId")]
        //public int? VersionTypeId { get; set; }

        //      [DataMember(Name = "hasPrevious")]
        //public bool? HasPrevious { get; set; }

        [DataMember(Name = "version")]
        public string Version { get; set; }

        [DataMember(Name = "totalScore")]
        public int? TotalScore { get; set; }
    }
}
