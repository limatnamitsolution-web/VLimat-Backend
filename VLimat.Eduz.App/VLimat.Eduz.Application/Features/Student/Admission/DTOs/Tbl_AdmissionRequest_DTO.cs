using Microsoft.AspNetCore.Http;

namespace VLimat.Eduz.Application.Features.Student.Admission.DTOs
{
    public class StudentAdmissionRequestDto
    {
        // --- Student Details (Tab 0) ---
        public int? adm_branch_Id { get; set; }
        public string adm_no { get; set; }
        public DateTime? adm_date { get; set; }
        public DateTime? adm_doj { get; set; }
        public string sess_stud_first_name { get; set; }
        public string sess_stud_last_name { get; set; }
        public string adm_ssr_no { get; set; }
        public DateTime? adm_dob { get; set; }
        public int? adm_gender_id { get; set; }
        public int? adm_blood_grp_id { get; set; }
        public int? sess_religion_id { get; set; }
        public int? sess_caste_id { get; set; }
        public string adm_stud_mobile_no { get; set; }
        public string sess_student_aadhar_no { get; set; }
        public string adm_stud_email_ddress { get; set; }

        // Address Info
        public int? sess_country_id { get; set; }
        public int? sess_state_id { get; set; }
        public int? sess_city_id { get; set; }
        public string sess_address { get; set; }
        public string sess_pin_code { get; set; }

        // Permanent Address Info
        public int? sess_permanent_country_id { get; set; }
        public int? sess_permanent_state_id { get; set; }
        public int? sess_permanent_city_id { get; set; }
        public string sess_permanent_address { get; set; }
        public string sess_permanent_pin_code { get; set; }

        // --- Academic Details (Tab 1) ---
        // Admission
        public int? adm_cat_id { get; set; }
        public int? adm_grp_id { get; set; }
        public int? adm_stream_id { get; set; }
        public int? adm_class_id { get; set; }
        public int? adm_section_id { get; set; }
        public string adm_rollno { get; set; }
        public int? adm_concession_id { get; set; }
        public int? adm_fee_group_id { get; set; }

        // Session
        public int? sess_cat_id { get; set; }
        public int? sess_grp_id { get; set; }
        public int? sess_stream_id { get; set; }
        public int? sess_class_id { get; set; }
        public int? sess_section_id { get; set; }
        public string sess_roll_no { get; set; }
        public int? sess_concession_id { get; set; }
        public int? sess_fee_group_id { get; set; }

        // --- Parent Details (Tab 2) ---
        // Father
        public string sess_father_name { get; set; }
        public string sess_father_mobile_no { get; set; }
        public int? sess_father_qualification_id { get; set; }
        public int? sess_father_occupation_id { get; set; }
        public string sess_father_designation_id { get; set; } // Input type text in HTML
        public string sess_father_annual_income { get; set; }
        public string sess_father_office_address { get; set; }
        public bool sess_is_fse { get; set; }

        // Mother
        public string sess_mother_name { get; set; }
        public string sess_mother_mobile_no { get; set; }
        public int? sess_mother_qualification_id { get; set; }
        public int? sess_mother_occupation_id { get; set; }
        public string sess_mother_designation_id { get; set; }
        public string sess_mother_annual_income { get; set; }
        public string sess_mother_office_address { get; set; }
        public bool sess_is_mse { get; set; }

        // Guardians
        public string sess_g1_name { get; set; }
        public string sess_g1_mobile_no { get; set; }
        public string sess_g1_address { get; set; }
        public string sess_g2_name { get; set; }
        public string sess_g2_mobile_no { get; set; }
        public string sess_g2_address { get; set; }

        public string otherDetails { get; set; }

        // --- Transport Details (Tab 3) ---
        public int? transportMode { get; set; }
        public int? pickDrop { get; set; }

        // Pick
        public int? pickArea { get; set; }
        public int? pickStand { get; set; }
        public int? pickRoute { get; set; }
        public int? pickDriver { get; set; }

        // Drop
        public int? dropArea { get; set; }
        public int? dropStand { get; set; }
        public int? dropRoute { get; set; }
        public int? dropDriver { get; set; }

        // Months (Array of Month IDs)
        public List<int> months { get; set; } = new List<int>();

        // --- Document Upload (Tab 4) ---
        public List<StudentDocumentRequestDto> Docs { get; set; } = new List<StudentDocumentRequestDto>();
    }

    public class StudentDocumentRequestDto
    {
        public int doc_id { get; set; }
        public string doc_Code { get; set; }
        public string doc_label { get; set; }

        // This property captures the file upload
        public IFormFile doc_File { get; set; }
    }

}
