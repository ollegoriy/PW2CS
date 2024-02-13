using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Newtonsoft.Json;

namespace PW2CS
{
    public partial class MainWindow : Window
    {
        private string notesFilePath = "notes.json";
        private List<Note> notes = new List<Note>();

        public MainWindow()
        {
            InitializeComponent();
            LoadNotes();
            kakoyden.SelectedDate = DateTime.Today;
            ShowNotes(DateTime.Today);
        }

        private void LoadNotes()
        {
            if (File.Exists(notesFilePath))
            {
                string json = File.ReadAllText(notesFilePath);
                notes = JsonConvert.DeserializeObject<List<Note>>(json);
            }
        }

        private void SaveNotes()
        {
            string json = JsonConvert.SerializeObject(notes);
            File.WriteAllText(notesFilePath, json);
        }

        private void ShowNotes(DateTime date)
        {
            zapiski.Items.Clear();
            foreach (var note in notes)
            {
                if (note.Date == date)
                {
                    zapiski.Items.Add(note.Title);
                }
            }
        }

        private void kakoydenSDC(object sender, RoutedEventArgs e)
        {
            ShowNotes(kakoyden.SelectedDate ?? DateTime.Today);
        }

        private void zapiskiSC(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (zapiski.SelectedIndex != -1)
            {
                int selectedIndex = zapiski.SelectedIndex;
                Note selectedNote = notes.Find(note => note.Title == zapiski.SelectedItem.ToString() && note.Date == kakoyden.SelectedDate);
                if (selectedNote != null)
                {
                    title.Text = selectedNote.Title;
                    desc.Text = selectedNote.Description;
                }
            }
        }

        private void dobavit(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(title.Text))
            {
                Note newNote = new Note
                {
                    Title = title.Text,
                    Description = desc.Text,
                    Date = kakoyden.SelectedDate ?? DateTime.Today
                };
                notes.Add(newNote);
                SaveNotes();
                ShowNotes(newNote.Date);
            }
        }

        private void izmenit(object sender, RoutedEventArgs e)
        {
            if (zapiski.SelectedIndex != -1)
            {
                int selectedIndex = zapiski.SelectedIndex;
                Note selectedNote = notes.Find(note => note.Title == zapiski.SelectedItem.ToString() && note.Date == kakoyden.SelectedDate);
                if (selectedNote != null)
                {
                    selectedNote.Title = title.Text;
                    selectedNote.Description = desc.Text;
                    SaveNotes();
                    ShowNotes(selectedNote.Date);
                }
            }
        }

        private void udalit(object sender, RoutedEventArgs e)
        {
            if (zapiski.SelectedIndex != -1)
            {
                int selectedIndex = zapiski.SelectedIndex;
                Note selectedNote = notes.Find(note => note.Title == zapiski.SelectedItem.ToString() && note.Date == kakoyden.SelectedDate);
                if (selectedNote != null)
                {
                    notes.Remove(selectedNote);
                    SaveNotes();
                    ShowNotes(selectedNote.Date);
                    title.Text = "";
                    desc.Text = "";
                }
            }
        }
    }

    public class Note
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}