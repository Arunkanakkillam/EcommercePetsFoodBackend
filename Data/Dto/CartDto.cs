﻿namespace EcommercePetsFoodBackend.Data.Dto
{
    public class CartDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }  
        public string Image {  get; set; }
        public int Quantity {  get; set; }
        public decimal Total {  get; set; }
    }
}
