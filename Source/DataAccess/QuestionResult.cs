//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class QuestionResult
    {
        public int answer_id { get; set; }
        public string answer_text { get; set; }
        public Nullable<int> question_id { get; set; }
        public string question_text { get; set; }
        public Nullable<int> question_group_id { get; set; }
        public Nullable<int> booking_id { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}