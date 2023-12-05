namespace LoLTournaments.Shared.Models
{

    public class FileUploadModel
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public byte[] Data { get; set; }

        public bool IsValid => !string.IsNullOrEmpty(Name) && Data is {Length: > 0};
    }

}