﻿namespace Expense_Tracker_2._0.Services.Interfaces
{
    public interface ICloudService
    {
        string UploadImage(IFormFile photo);

        void DeleteImage();
    }
}
