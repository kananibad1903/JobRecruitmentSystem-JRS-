namespace JobRecruitmentSystem.UI.Constants
{
    public static class JobPostOptions
    {
        public static readonly string[] Categories = new[]
        {
            "IT və Proqramlaşdırma",
            "Dizayn və Qrafika",
            "Marketinq",
            "Satış",
            "Maliyyə və Mühasibatlıq",
            "Tikinti",
            "Təhsil",
            "Səhiyyə",
            "Logistika və Nəqliyyat",
            "Müştəri Xidmətləri",
            "İnsan Resursları",
            "Hüquq",
            "Turizm və Otelçilik",
            "İstehsalat",
        };

        public static readonly string[] JobTypes = new[]
        {
            "Tam ştat",
            "Yarım ştat",
            "Uzaqdan (Remote)",
            "Sərbəst (Freelance)",
            "Təcrübə (Internship)",
        };

        public static readonly string[] Districts = new[]
        {
            "Binəqədi",
            "Qaradağ",
            "Nərimanov",
            "Nəsimi",
            "Nizami",
            "Pirallahı",
            "Sabunçu",
            "Səbail",
            "Suraxanı",
            "Xətai",
            "Xəzər",
            "Yasamal",
        };

        public static readonly Dictionary<string, string> CategoryKeys = new()
        {
            ["IT və Proqramlaşdırma"] = "category.it",
            ["Dizayn və Qrafika"] = "category.design",
            ["Marketinq"] = "category.marketing",
            ["Satış"] = "category.sales",
            ["Maliyyə və Mühasibatlıq"] = "category.finance",
            ["Tikinti"] = "category.construction",
            ["Təhsil"] = "category.education",
            ["Səhiyyə"] = "category.healthcare",
            ["Logistika və Nəqliyyat"] = "category.logistics",
            ["Müştəri Xidmətləri"] = "category.customerService",
            ["İnsan Resursları"] = "category.hr",
            ["Hüquq"] = "category.legal",
            ["Turizm və Otelçilik"] = "category.tourism",
            ["İstehsalat"] = "category.manufacturing",
        };

        public static readonly Dictionary<string, string> JobTypeKeys = new()
        {
            ["Tam ştat"] = "jobtype.fullTime",
            ["Yarım ştat"] = "jobtype.partTime",
            ["Uzaqdan (Remote)"] = "jobtype.remote",
            ["Sərbəst (Freelance)"] = "jobtype.freelance",
            ["Təcrübə (Internship)"] = "jobtype.internship",
        };

        public static readonly Dictionary<string, string> DistrictKeys = new()
        {
            ["Binəqədi"] = "district.binagadi",
            ["Qaradağ"] = "district.qaradag",
            ["Nərimanov"] = "district.narimanov",
            ["Nəsimi"] = "district.nasimi",
            ["Nizami"] = "district.nizami",
            ["Pirallahı"] = "district.pirallahi",
            ["Sabunçu"] = "district.sabunchu",
            ["Səbail"] = "district.sabail",
            ["Suraxanı"] = "district.surakhani",
            ["Xətai"] = "district.khatai",
            ["Xəzər"] = "district.khazar",
            ["Yasamal"] = "district.yasamal",
        };
    }
}
