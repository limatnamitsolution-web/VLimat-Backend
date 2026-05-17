using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VLimat.Eduz.Application.Features.Student.Admission.DTOs
{
    public class Tbl_AdmissionResponse_DTO
    {
        [Key]
        public int adm_id { get; set; }

        [StringLength(10)]
        public string adm_fn_fy { get; set; }

        public int adm_branch_Id { get; set; }

        public int adm_cat_id { get; set; }

        [StringLength(10)]
        public string adm_temp_adm_no { get; set; }

        [StringLength(10)]
        public string adm_no { get; set; }

        public DateTime? adm_date { get; set; }

        [StringLength(20)]
        public string adm_prospectus_no { get; set; }

        public int adm_class_id { get; set; }

        public int adm_semester_id { get; set; }

        public int adm_section_id { get; set; }

        [StringLength(20)]
        public string adm_rollno { get; set; }

        public int adm_stream_id { get; set; }

        public int adm_gender_id { get; set; }

        public DateTime? adm_dob { get; set; }

        [StringLength(30)]
        public string adm_dob_in_words { get; set; }

        [StringLength(30)]
        public string adm_age_yy_mm_dd { get; set; }

        [StringLength(15)]
        public string adm_contact_no { get; set; }

        [StringLength(15)]
        public string adm_emg_contact_no { get; set; }

        [StringLength(200)]
        public string adm_sms_alert { get; set; }

        [StringLength(200)]
        public string adm_stud_pre_school_name { get; set; }

        [StringLength(200)]
        public string adm_stud_pre_school_class { get; set; }

        [StringLength(200)]
        public string adm_stud_pre_school_percent { get; set; }

        public DateTime? adm_stud_pre_school_tc_date { get; set; }

        [StringLength(200)]
        public string adm_father_address { get; set; }

        [StringLength(200)]
        public string adm_mother_address { get; set; }

        public int adm_blood_grp_id { get; set; }

        public int adm_language_id { get; set; }

        public int adm_type_id { get; set; }

        public DateTime? adm_created_date { get; set; }

        public bool adm_is_studying { get; set; }

        public bool adm_is_transfer_case { get; set; }

        public int adm_user_id { get; set; }

        public bool adm_is_cancel { get; set; }

        public bool adm_is_cancel_user_id { get; set; }

        [StringLength(200)]
        public string adm_remarks { get; set; }

        [StringLength(200)]
        public string adm_other_details_fany { get; set; }

        public long adm_document_id { get; set; }

        public DateTime? adm_cancel_date { get; set; }

        [StringLength(200)]
        public string adm_ssr_no { get; set; }

        [StringLength(200)]
        public string adm_moi_no { get; set; }

        [StringLength(200)]
        public string adm_stud_pwd { get; set; }

        public int adm_disabilityType_id { get; set; }

        public bool adm_is_adopted_child { get; set; }

        public long adm_stud_user_id { get; set; }

        public long adm_extra_activity_id { get; set; }

        public bool adm_is_app_active { get; set; }

        [StringLength(200)]
        public string adm_stud_photo_url { get; set; }

        [StringLength(200)]
        public string adm_father_photo_url { get; set; }

        [StringLength(200)]
        public string adm_mother_photo_url { get; set; }

        [StringLength(200)]
        public string adm_g_photo_url { get; set; }

        [StringLength(50)]
        public string adm_attendance { get; set; }

        public DateTime? adm_doj { get; set; }

        [StringLength(300)]
        public string adm_stud_pre_school_address { get; set; }

        [StringLength(300)]
        public string adm_stud_email_ddress { get; set; }

        [StringLength(15)]
        public string adm_stud_mobile_no { get; set; }

        [StringLength(50)]
        public string adm_block_equest { get; set; }

        public long adm_fn_fy_id { get; set; }

        public bool adm_is_incharge { get; set; }

        [StringLength(50)]
        public string adm_after_10th { get; set; }

        [StringLength(50)]
        public string adm_result_status { get; set; }

        [StringLength(50)]
        public string adm_sersno { get; set; }

        public DateTime? adm_stud_pwd_Modified { get; set; }

        [StringLength(50)]
        public string adm_udise { get; set; }

        [StringLength(50)]
        public string adm_apaar_id { get; set; }
    }
}
