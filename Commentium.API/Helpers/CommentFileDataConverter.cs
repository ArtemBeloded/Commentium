namespace Commentium.API.Helpers
{
    public static class CommentFileDataConverter
    {
        public static byte[] ConvertCommentFileToByteArray(IFormFile file) 
        {
            using var stream = file.OpenReadStream();
            using var reader = new BinaryReader(stream);
            using var outputStream = new MemoryStream();

            byte[] buffer = new byte[4096];
            int bytesRead;

            while ((bytesRead = reader.Read(buffer, 0, buffer.Length)) > 0)
            {
                outputStream.Write(buffer, 0, bytesRead);
            }

            return outputStream.ToArray();
        }
    }
}
