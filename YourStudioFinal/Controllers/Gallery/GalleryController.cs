﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourStudioFinal.data;
using YourStudioFinal.Models;
using YourStudioFinal.Models.Gallery;

namespace YourStudioFinal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GalleryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public GalleryController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> CoupleAdminIndex()
        {
            var UserDetails = await _userManager.GetUserAsync(User);
            if (UserDetails != null)
            {
                ViewBag.User = UserDetails;
                ViewBag.isLogged = true;
            }
            else
            {
                ViewBag.isLogged = false;
            }
            
            var gallery = await _context.Gallery.FirstOrDefaultAsync();

            if (gallery == null)
            {
                gallery = new Models.Gallery.Gallery();
                gallery.Id = Guid.NewGuid().ToString();
                gallery.GalleryFiles = new();
                _context.Gallery.Add(gallery);
                await _context.SaveChangesAsync();
                gallery = await _context.Gallery.FirstOrDefaultAsync();
            }
            else
            {
                var files = _context.GalleryFiles.Where(e => e.GalleryId == gallery.Id).OrderByDescending(x => x.DateUploaded)
                    .Where(x => x.category == "Couples");
                gallery.GalleryFiles = files.ToList();
            }

            return View(gallery);
        }
        
        public async Task<IActionResult> FamilyAdminIndex()
        {
            var UserDetails = await _userManager.GetUserAsync(User);
            if (UserDetails != null)
            {
                ViewBag.User = UserDetails;
                ViewBag.isLogged = true;
            }
            else
            {
                ViewBag.isLogged = false;
            }
            
            var gallery = await _context.Gallery.FirstOrDefaultAsync();

            if (gallery == null)
            {
                gallery = new Models.Gallery.Gallery();
                gallery.Id = Guid.NewGuid().ToString();
                gallery.GalleryFiles = new();
                _context.Gallery.Add(gallery);
                await _context.SaveChangesAsync();
                gallery = await _context.Gallery.FirstOrDefaultAsync();
            }
            else
            {
                var files = _context.GalleryFiles.Where(e => e.GalleryId == gallery.Id).OrderByDescending(x => x.DateUploaded)
                    .Where(x => x.category == "Family");
                gallery.GalleryFiles = files.ToList();
            }

            return View(gallery);
        }
        
        public async Task<IActionResult> FurAdminIndex()
        {
            var UserDetails = await _userManager.GetUserAsync(User);
            if (UserDetails != null)
            {
                ViewBag.User = UserDetails;
                ViewBag.isLogged = true;
            }
            else
            {
                ViewBag.isLogged = false;
            }
            
            var gallery = await _context.Gallery.FirstOrDefaultAsync();

            if (gallery == null)
            {
                gallery = new Models.Gallery.Gallery();
                gallery.Id = Guid.NewGuid().ToString();
                gallery.GalleryFiles = new();
                _context.Gallery.Add(gallery);
                await _context.SaveChangesAsync();
                gallery = await _context.Gallery.FirstOrDefaultAsync();
            }
            else
            {
                var files = _context.GalleryFiles.Where(e => e.GalleryId == gallery.Id).OrderByDescending(x => x.DateUploaded)
                    .Where(x => x.category == "Fur Families");
                gallery.GalleryFiles = files.ToList();
            }

            return View(gallery);
        }
        
        public async Task<IActionResult> SoloAdminIndex()
        {
            var UserDetails = await _userManager.GetUserAsync(User);
            if (UserDetails != null)
            {
                ViewBag.User = UserDetails;
                ViewBag.isLogged = true;
            }
            else
            {
                ViewBag.isLogged = false;
            }
            
            var gallery = await _context.Gallery.FirstOrDefaultAsync();

            if (gallery == null)
            {
                gallery = new Models.Gallery.Gallery();
                gallery.Id = Guid.NewGuid().ToString();
                gallery.GalleryFiles = new();
                _context.Gallery.Add(gallery);
                await _context.SaveChangesAsync();
                gallery = await _context.Gallery.FirstOrDefaultAsync();
            }
            else
            {
                var files = _context.GalleryFiles.Where(e => e.GalleryId == gallery.Id).OrderByDescending(x => x.DateUploaded).Where(x => x.category == "Solo");
                gallery.GalleryFiles = files.ToList();
            }

            return View(gallery);
        }

        public async Task<IActionResult> Index()
        {
            var UserDetails = await _userManager.GetUserAsync(User);
            if (UserDetails != null)
            {
                ViewBag.User = UserDetails;
                ViewBag.isLogged = true;
            }
            else
            {
                ViewBag.isLogged = false;
            }
            
            var gallery = await _context.Gallery.FirstOrDefaultAsync();

            if (gallery == null)
            {
                gallery = new Models.Gallery.Gallery();
                gallery.Id = Guid.NewGuid().ToString();
                gallery.GalleryFiles = new();
                _context.Gallery.Add(gallery);
                await _context.SaveChangesAsync();
                gallery = await _context.Gallery.FirstOrDefaultAsync();
            }
            else
            {
                var files = _context.GalleryFiles.Where(e => e.GalleryId == gallery.Id).OrderByDescending(x => x.DateUploaded);
                gallery.GalleryFiles = files.ToList();
            }

            return View(gallery);
        }

        public async Task<IActionResult> Upload([FromForm(Name = "image")] IFormFileCollection files, [FromForm(Name="category")] string formCategory)
        {
            // TODO link to user data
            var currentGallery = await _context.Gallery.FirstOrDefaultAsync();

            // list of files to add to the gallery
            List<GalleryFile> f = new List<GalleryFile>();

            if (files.Count == 0)
                return RedirectToAction("Index");

            // Ensure that the uploads directory exists
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", currentGallery.Id);
            if (!Path.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            foreach (var file in files)
            {
                if (file.Length > int.MaxValue)
                {
                    ViewBag.Error = "File is too large";
                    return RedirectToAction("Index");
                }
            }

            foreach (var file in files)
            {
                var _path = Path.Combine(filePath, file.FileName);
                await using (var fileStream = new FileStream(_path, FileMode.Create))
                {
                    // Create the file...
                    await file.CopyToAsync(fileStream);

                    // Create an entry into the database
                    var galleryFile = new GalleryFile();

                    // Set the file id and the gallery it's attached to
                    galleryFile.Id = Guid.NewGuid().ToString();
                    galleryFile.fileName = Path.Combine(currentGallery.Id, file.FileName);
                    galleryFile.Gallery = currentGallery;
                    galleryFile.category = formCategory;
                    galleryFile.DateUploaded = DateTime.Now;

                    // Add the files to the gallery dbset
                    _context.GalleryFiles.Add(galleryFile);

                    // Add the file to the list of files to add to the gallery
                    f.Add(galleryFile);

                    // Ensure the file is written to disk
                    await fileStream.FlushAsync();
                }
            }

            // Save the gallery
            currentGallery.GalleryFiles = f;
            _context.Gallery.Update(currentGallery);
            

            // Save and reload page
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public async Task<IActionResult> Delete(string ids)
        {
            var idList = ids.Split(",");
            
            
            foreach (var id in idList)
            {
                var galleryFile = await _context.GalleryFiles.FirstOrDefaultAsync(e => e.Id == id);
                if (galleryFile != null)
                {
                    _context.GalleryFiles.Remove(galleryFile);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("Index");
        }
        
    }
}