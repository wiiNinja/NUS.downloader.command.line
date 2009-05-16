using System;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace NUS_Downloader
{
    public partial class Form1 : Form
    {
        const string NUSURL = "http://nus.cdn.shop.wii.com/ccs/download/";
        const string DSiNUSURL = "http://nus.cdn.t.shop.nintendowifi.net/ccs/download/";
        string version = "v1";
        WebClient generalWC = new WebClient();
        //private TextBox titleidbox;
        //private TextBox titleversion;
        public TextBox titleidbox;
        public TextBox titleversion;
        private RichTextBox statusbox;
        private Button button1;
        private Button button2;
        private Button button3;
        //private CheckBox packbox;
        //private CheckBox localuse;
        public CheckBox packbox;
        public CheckBox localuse;
        //private RadioButton radioButton1;
        //private RadioButton radioButton2;
        public RadioButton radioButton1Wii;
        public RadioButton radioButton2DS;
        private ProgressBar dlprogress;
        
        public struct WADHeader {
	        public int HeaderSize;
            public int WadType;
            public int CertChainSize;
            public int Reserved;
            public int TicketSize;
            public int TMDSize;
            public int DataSize;
            public int FooterSize;
        };

        byte[] cert = new byte[2560]
        {
            0x00, 0x01, 0x00, 0x01, 0x7D, 0x9D, 0x5E, 0xBA, 0x52, 0x81, 0xDC, 0xA7, 0x06, 0x5D, 0x2F, 0x08, 
            0x68, 0xDB, 0x8A, 0xC7, 0x3A, 0xCE, 0x7E, 0xA9, 0x91, 0xF1, 0x96, 0x9F, 0xE1, 0xD0, 0xF2, 0xC1, 
            0x1F, 0xAE, 0xC0, 0xC3, 0xF0, 0x1A, 0xDC, 0xB4, 0x46, 0xAD, 0xE5, 0xCA, 0x03, 0xB6, 0x25, 0x21, 
            0x94, 0x62, 0xC6, 0xE1, 0x41, 0x0D, 0xB9, 0xE6, 0x3F, 0xDE, 0x98, 0xD1, 0xAF, 0x26, 0x3B, 0x4C, 
            0xB2, 0x87, 0x84, 0x27, 0x82, 0x72, 0xEF, 0x27, 0x13, 0x4B, 0x87, 0xC2, 0x58, 0xD6, 0x7B, 0x62, 
            0xF2, 0xB5, 0xBF, 0x9C, 0xB6, 0xBA, 0x8C, 0x89, 0x19, 0x2E, 0xC5, 0x06, 0x89, 0xAC, 0x74, 0x24, 
            0xA0, 0x22, 0x09, 0x40, 0x03, 0xEE, 0x98, 0xA4, 0xBD, 0x2F, 0x01, 0x3B, 0x59, 0x3F, 0xE5, 0x66, 
            0x6C, 0xD5, 0xEB, 0x5A, 0xD7, 0xA4, 0x93, 0x10, 0xF3, 0x4E, 0xFB, 0xB4, 0x3D, 0x46, 0xCB, 0xF1, 
            0xB5, 0x23, 0xCF, 0x82, 0xF6, 0x8E, 0xB5, 0x6D, 0xB9, 0x04, 0xA7, 0xC2, 0xA8, 0x2B, 0xE1, 0x1D, 
            0x78, 0xD3, 0x9B, 0xA2, 0x0D, 0x90, 0xD3, 0x07, 0x42, 0xDB, 0x5E, 0x7A, 0xC1, 0xEF, 0xF2, 0x21, 
            0x51, 0x09, 0x62, 0xCF, 0xA9, 0x14, 0xA8, 0x80, 0xDC, 0xF4, 0x17, 0xBA, 0x99, 0x93, 0x0A, 0xEE, 
            0x08, 0xB0, 0xB0, 0xE5, 0x1A, 0x3E, 0x9F, 0xAF, 0xCD, 0xC2, 0xD7, 0xE3, 0xCB, 0xA1, 0x2F, 0x3A, 
            0xC0, 0x07, 0x90, 0xDE, 0x44, 0x7A, 0xC3, 0xC5, 0x38, 0xA8, 0x67, 0x92, 0x38, 0x07, 0x8B, 0xD4, 
            0xC4, 0xB2, 0x45, 0xAC, 0x29, 0x16, 0x88, 0x6D, 0x2A, 0x0E, 0x59, 0x4E, 0xED, 0x5C, 0xC8, 0x35, 
            0x69, 0x8B, 0x4D, 0x62, 0x38, 0xDF, 0x05, 0x72, 0x4D, 0xCC, 0xF6, 0x81, 0x80, 0x8A, 0x70, 0x74, 
            0x06, 0x59, 0x30, 0xBF, 0xF8, 0x51, 0x41, 0x37, 0xE8, 0x15, 0xFA, 0xBA, 0xA1, 0x72, 0xB8, 0xE0, 
            0x69, 0x6C, 0x61, 0xE4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x52, 0x6F, 0x6F, 0x74, 0x2D, 0x43, 0x41, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x31, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x01, 0x58, 0x53, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x33, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0xF1, 0xB8, 0x9F, 0xD1, 0xAD, 0x07, 0xA9, 0x37, 0x8A, 0x7B, 0x10, 0x0C, 
            0x7D, 0xC7, 0x39, 0xBE, 0x9E, 0xDD, 0xB7, 0x32, 0x00, 0x89, 0xAB, 0x25, 0xB1, 0xF8, 0x71, 0xAF, 
            0x5A, 0xA9, 0xF4, 0x58, 0x9E, 0xD1, 0x83, 0x02, 0x32, 0x8E, 0x81, 0x1A, 0x1F, 0xEF, 0xD0, 0x09, 
            0xC8, 0x06, 0x36, 0x43, 0xF8, 0x54, 0xB9, 0xE1, 0x3B, 0xBB, 0x61, 0x3A, 0x7A, 0xCF, 0x87, 0x14, 
            0x85, 0x6B, 0xA4, 0x5B, 0xAA, 0xE7, 0xBB, 0xC6, 0x4E, 0xB2, 0xF7, 0x5D, 0x87, 0xEB, 0xF2, 0x67, 
            0xED, 0x0F, 0xA4, 0x41, 0xA9, 0x33, 0x66, 0x5E, 0x57, 0x7D, 0x5A, 0xDE, 0xAB, 0xFB, 0x46, 0x2E, 
            0x76, 0x00, 0xCA, 0x9C, 0xE9, 0x4D, 0xC4, 0xCB, 0x98, 0x39, 0x92, 0xAB, 0x7A, 0x2F, 0xB3, 0xA3, 
            0x9E, 0xA2, 0xBF, 0x9C, 0x53, 0xEC, 0xD0, 0xDC, 0xFA, 0x6B, 0x8B, 0x5E, 0xB2, 0xCB, 0xA4, 0x0F, 
            0xFA, 0x40, 0x75, 0xF8, 0xF2, 0xB2, 0xDE, 0x97, 0x38, 0x11, 0x87, 0x2D, 0xF5, 0xE2, 0xA6, 0xC3, 
            0x8B, 0x2F, 0xDC, 0x8E, 0x57, 0xDD, 0xBD, 0x5F, 0x46, 0xEB, 0x27, 0xD6, 0x19, 0x52, 0xF6, 0xAE, 
            0xF8, 0x62, 0xB7, 0xEE, 0x9A, 0xC6, 0x82, 0xA2, 0xB1, 0x9A, 0xA9, 0xB5, 0x58, 0xFB, 0xEB, 0xB3, 
            0x89, 0x2F, 0xBD, 0x50, 0xC9, 0xF5, 0xDC, 0x4A, 0x6E, 0x9C, 0x9B, 0xFE, 0x45, 0x80, 0x34, 0xA9, 
            0x42, 0x18, 0x2D, 0xDE, 0xB7, 0x5F, 0xE0, 0xD1, 0xB3, 0xDF, 0x0E, 0x97, 0xE3, 0x99, 0x80, 0x87, 
            0x70, 0x18, 0xC2, 0xB2, 0x83, 0xF1, 0x35, 0x75, 0x7C, 0x5A, 0x30, 0xFC, 0x3F, 0x30, 0x84, 0xA4, 
            0x9A, 0xAA, 0xC0, 0x1E, 0xE7, 0x06, 0x69, 0x4F, 0x8E, 0x14, 0x48, 0xDA, 0x12, 0x3A, 0xCC, 0x4F, 
            0xFA, 0x26, 0xAA, 0x38, 0xF7, 0xEF, 0xBF, 0x27, 0x8F, 0x36, 0x97, 0x79, 0x77, 0x5D, 0xB7, 0xC5, 
            0xAD, 0xC7, 0x89, 0x91, 0xDC, 0xF8, 0x43, 0x8D, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x01, 0x00, 0x00, 0xB3, 0xAD, 0xB3, 0x22, 0x6B, 0x3C, 0x3D, 0xFF, 0x1B, 0x4B, 0x40, 0x77, 
            0x16, 0xFF, 0x4F, 0x7A, 0xD7, 0x64, 0x86, 0xC8, 0x95, 0xAC, 0x56, 0x2D, 0x21, 0xF1, 0x06, 0x01, 
            0xD4, 0xF6, 0x64, 0x28, 0x19, 0x1C, 0x07, 0x76, 0x8F, 0xDF, 0x1A, 0xE2, 0xCE, 0x7B, 0x27, 0xC9, 
            0x0F, 0xBC, 0x0A, 0xD0, 0x31, 0x25, 0x78, 0xEC, 0x07, 0x79, 0xB6, 0x57, 0xD4, 0x37, 0x24, 0x13, 
            0xA7, 0xF8, 0x6F, 0x0C, 0x14, 0xC0, 0xEF, 0x6E, 0x09, 0x41, 0xED, 0x2B, 0x05, 0xEC, 0x39, 0x57, 
            0x36, 0x07, 0x89, 0x00, 0x4A, 0x87, 0x8D, 0x2E, 0x9D, 0xF8, 0xC7, 0xA5, 0xA9, 0xF8, 0xCA, 0xB3, 
            0x11, 0xB1, 0x18, 0x79, 0x57, 0xBB, 0xF8, 0x98, 0xE2, 0xA2, 0x54, 0x02, 0xCF, 0x54, 0x39, 0xCF, 
            0x2B, 0xBF, 0xA0, 0xE1, 0xF8, 0x5C, 0x06, 0x6E, 0x83, 0x9A, 0xE0, 0x94, 0xCA, 0x47, 0xE0, 0x15, 
            0x58, 0xF5, 0x6E, 0x6F, 0x34, 0xE9, 0x2A, 0xA2, 0xDC, 0x38, 0x93, 0x7E, 0x37, 0xCD, 0x8C, 0x5C, 
            0x4D, 0xFD, 0x2F, 0x11, 0x4F, 0xE8, 0x68, 0xC9, 0xA8, 0xD9, 0xFE, 0xD8, 0x6E, 0x0C, 0x21, 0x75, 
            0xA2, 0xBD, 0x7E, 0x89, 0xB9, 0xC7, 0xB5, 0x13, 0xF4, 0x1A, 0x79, 0x61, 0x44, 0x39, 0x10, 0xEF, 
            0xF9, 0xD7, 0xFE, 0x57, 0x22, 0x18, 0xD5, 0x6D, 0xFB, 0x7F, 0x49, 0x7A, 0xA4, 0xCB, 0x90, 0xD4, 
            0xF1, 0xAE, 0xB1, 0x76, 0xE4, 0x68, 0x5D, 0xA7, 0x94, 0x40, 0x60, 0x98, 0x2F, 0x04, 0x48, 0x40, 
            0x1F, 0xCF, 0xC6, 0xBA, 0xEB, 0xDA, 0x16, 0x30, 0xB4, 0x73, 0xB4, 0x15, 0x23, 0x35, 0x08, 0x07, 
            0x0A, 0x9F, 0x4F, 0x89, 0x78, 0xE6, 0x2C, 0xEC, 0x5E, 0x92, 0x46, 0xA5, 0xA8, 0xBD, 0xA0, 0x85, 
            0x78, 0x68, 0x75, 0x0C, 0x3A, 0x11, 0x2F, 0xAF, 0x95, 0xE8, 0x38, 0xC8, 0x99, 0x0E, 0x87, 0xB1, 
            0x62, 0xCD, 0x10, 0xDA, 0xB3, 0x31, 0x96, 0x65, 0xEF, 0x88, 0x9B, 0x54, 0x1B, 0xB3, 0x36, 0xBB, 
            0x67, 0x53, 0x9F, 0xAF, 0xC2, 0xAE, 0x2D, 0x0A, 0x2E, 0x75, 0xC0, 0x23, 0x74, 0xEA, 0x4E, 0xAC, 
            0x8D, 0x99, 0x50, 0x7F, 0x59, 0xB9, 0x53, 0x77, 0x30, 0x5F, 0x26, 0x35, 0xC6, 0x08, 0xA9, 0x90, 
            0x93, 0xAC, 0x8F, 0xC6, 0xDE, 0x23, 0xB9, 0x7A, 0xEA, 0x70, 0xB4, 0xC4, 0xCF, 0x66, 0xB3, 0x0E, 
            0x58, 0x32, 0x0E, 0xC5, 0xB6, 0x72, 0x04, 0x48, 0xCE, 0x3B, 0xB1, 0x1C, 0x53, 0x1F, 0xCB, 0x70, 
            0x28, 0x7C, 0xB5, 0xC2, 0x7C, 0x67, 0x4F, 0xBB, 0xFD, 0x8C, 0x7F, 0xC9, 0x42, 0x20, 0xA4, 0x73, 
            0x23, 0x1D, 0x58, 0x7E, 0x5A, 0x1A, 0x1A, 0x82, 0xE3, 0x75, 0x79, 0xA1, 0xBB, 0x82, 0x6E, 0xCE, 
            0x01, 0x71, 0xC9, 0x75, 0x63, 0x47, 0x4B, 0x1D, 0x46, 0xE6, 0x79, 0xB2, 0x82, 0x37, 0x62, 0x11, 
            0xCD, 0xC7, 0x00, 0x2F, 0x46, 0x87, 0xC2, 0x3C, 0x6D, 0xC0, 0xD5, 0xB5, 0x78, 0x6E, 0xE1, 0xF2, 
            0x73, 0xFF, 0x01, 0x92, 0x50, 0x0F, 0xF4, 0xC7, 0x50, 0x6A, 0xEE, 0x72, 0xB6, 0xF4, 0x3D, 0xF6, 
            0x08, 0xFE, 0xA5, 0x83, 0xA1, 0xF9, 0x86, 0x0F, 0x87, 0xAF, 0x52, 0x44, 0x54, 0xBB, 0x47, 0xC3, 
            0x06, 0x0C, 0x94, 0xE9, 0x9B, 0xF7, 0xD6, 0x32, 0xA7, 0xC8, 0xAB, 0x4B, 0x4F, 0xF5, 0x35, 0x21, 
            0x1F, 0xC1, 0x80, 0x47, 0xBB, 0x7A, 0xFA, 0x5A, 0x2B, 0xD7, 0xB8, 0x84, 0xAD, 0x8E, 0x56, 0x4F, 
            0x5B, 0x89, 0xFF, 0x37, 0x97, 0x37, 0xF1, 0xF5, 0x01, 0x3B, 0x1F, 0x9E, 0xC4, 0x18, 0x6F, 0x92, 
            0x2A, 0xD5, 0xC4, 0xB3, 0xC0, 0xD5, 0x87, 0x0B, 0x9C, 0x04, 0xAF, 0x1A, 0xB5, 0xF3, 0xBC, 0x6D, 
            0x0A, 0xF1, 0x7D, 0x47, 0x08, 0xE4, 0x43, 0xE9, 0x73, 0xF7, 0xB7, 0x70, 0x77, 0x54, 0xBA, 0xF3, 
            0xEC, 0xD2, 0xAC, 0x49, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x52, 0x6F, 0x6F, 0x74, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x01, 0x43, 0x41, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x31, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x5B, 0xFA, 0x7D, 0x5C, 0xB2, 0x79, 0xC9, 0xE2, 0xEE, 0xE1, 0x21, 0xC6, 
            0xEA, 0xF4, 0x4F, 0xF6, 0x39, 0xF8, 0x8F, 0x07, 0x8B, 0x4B, 0x77, 0xED, 0x9F, 0x95, 0x60, 0xB0, 
            0x35, 0x82, 0x81, 0xB5, 0x0E, 0x55, 0xAB, 0x72, 0x11, 0x15, 0xA1, 0x77, 0x70, 0x3C, 0x7A, 0x30, 
            0xFE, 0x3A, 0xE9, 0xEF, 0x1C, 0x60, 0xBC, 0x1D, 0x97, 0x46, 0x76, 0xB2, 0x3A, 0x68, 0xCC, 0x04, 
            0xB1, 0x98, 0x52, 0x5B, 0xC9, 0x68, 0xF1, 0x1D, 0xE2, 0xDB, 0x50, 0xE4, 0xD9, 0xE7, 0xF0, 0x71, 
            0xE5, 0x62, 0xDA, 0xE2, 0x09, 0x22, 0x33, 0xE9, 0xD3, 0x63, 0xF6, 0x1D, 0xD7, 0xC1, 0x9F, 0xF3, 
            0xA4, 0xA9, 0x1E, 0x8F, 0x65, 0x53, 0xD4, 0x71, 0xDD, 0x7B, 0x84, 0xB9, 0xF1, 0xB8, 0xCE, 0x73, 
            0x35, 0xF0, 0xF5, 0x54, 0x05, 0x63, 0xA1, 0xEA, 0xB8, 0x39, 0x63, 0xE0, 0x9B, 0xE9, 0x01, 0x01, 
            0x1F, 0x99, 0x54, 0x63, 0x61, 0x28, 0x70, 0x20, 0xE9, 0xCC, 0x0D, 0xAB, 0x48, 0x7F, 0x14, 0x0D, 
            0x66, 0x26, 0xA1, 0x83, 0x6D, 0x27, 0x11, 0x1F, 0x20, 0x68, 0xDE, 0x47, 0x72, 0x14, 0x91, 0x51, 
            0xCF, 0x69, 0xC6, 0x1B, 0xA6, 0x0E, 0xF9, 0xD9, 0x49, 0xA0, 0xF7, 0x1F, 0x54, 0x99, 0xF2, 0xD3, 
            0x9A, 0xD2, 0x8C, 0x70, 0x05, 0x34, 0x82, 0x93, 0xC4, 0x31, 0xFF, 0xBD, 0x33, 0xF6, 0xBC, 0xA6, 
            0x0D, 0xC7, 0x19, 0x5E, 0xA2, 0xBC, 0xC5, 0x6D, 0x20, 0x0B, 0xAF, 0x6D, 0x06, 0xD0, 0x9C, 0x41, 
            0xDB, 0x8D, 0xE9, 0xC7, 0x20, 0x15, 0x4C, 0xA4, 0x83, 0x2B, 0x69, 0xC0, 0x8C, 0x69, 0xCD, 0x3B, 
            0x07, 0x3A, 0x00, 0x63, 0x60, 0x2F, 0x46, 0x2D, 0x33, 0x80, 0x61, 0xA5, 0xEA, 0x6C, 0x91, 0x5C, 
            0xD5, 0x62, 0x35, 0x79, 0xC3, 0xEB, 0x64, 0xCE, 0x44, 0xEF, 0x58, 0x6D, 0x14, 0xBA, 0xAA, 0x88, 
            0x34, 0x01, 0x9B, 0x3E, 0xEB, 0xEE, 0xD3, 0x79, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x01, 0x00, 0x01, 0x4E, 0x00, 0x5F, 0xF1, 0x3F, 0x86, 0x75, 0x8D, 0xB6, 0x9C, 0x45, 0x63, 
            0x0F, 0xD4, 0x9B, 0xF4, 0xCC, 0x5D, 0x54, 0xCF, 0xCC, 0x22, 0x34, 0x72, 0x57, 0xAB, 0xA4, 0xBA, 
            0x53, 0xD2, 0xB3, 0x3D, 0xE6, 0xEC, 0x9E, 0xA1, 0x57, 0x54, 0x53, 0xAE, 0x5F, 0x93, 0x3D, 0x96, 
            0xBF, 0xF7, 0xCC, 0x7A, 0x79, 0x56, 0x6E, 0x84, 0x7B, 0x1B, 0x60, 0x77, 0xC2, 0xA9, 0x38, 0x71, 
            0x30, 0x1A, 0x8C, 0xD3, 0xC9, 0x3D, 0x4D, 0xB3, 0x26, 0xE9, 0x87, 0x92, 0x66, 0xE9, 0xD3, 0xBA, 
            0x9F, 0x79, 0xBC, 0x46, 0x38, 0xFA, 0x2D, 0x20, 0xA0, 0x3A, 0x70, 0x67, 0xA4, 0x11, 0xA7, 0xA0, 
            0xB7, 0xD9, 0x12, 0xAD, 0x11, 0x6A, 0x3A, 0xC4, 0x6E, 0x32, 0x42, 0x47, 0xC2, 0x08, 0xBA, 0xB4, 
            0x94, 0x9C, 0xC5, 0x2E, 0xD0, 0x2F, 0x19, 0xF6, 0x51, 0xE0, 0xDF, 0x2E, 0x36, 0x53, 0xAA, 0xAF, 
            0x97, 0xA6, 0x92, 0xBB, 0xA9, 0x1D, 0xD8, 0x6E, 0x24, 0x2E, 0xB3, 0x08, 0x77, 0x55, 0x11, 0xCE, 
            0x98, 0xF6, 0xA2, 0xF4, 0x26, 0xC9, 0x27, 0x04, 0xD0, 0xFC, 0x8D, 0xD4, 0x80, 0x9E, 0xD7, 0x61, 
            0xBD, 0x11, 0xB7, 0x85, 0x94, 0x8C, 0xD6, 0xD0, 0x7A, 0xDB, 0xA4, 0x08, 0xD0, 0xF0, 0x86, 0xF6, 
            0x5A, 0xAE, 0x19, 0x14, 0xB2, 0x88, 0x9A, 0xA8, 0xAE, 0x4A, 0xA2, 0xAA, 0xC7, 0x61, 0xA9, 0x0D, 
            0x41, 0x2C, 0xB1, 0x50, 0x09, 0xAB, 0x3E, 0x93, 0xFC, 0xA9, 0x24, 0xDE, 0xCE, 0x4F, 0x7C, 0x06, 
            0xAB, 0xDC, 0x2E, 0x60, 0x9D, 0x68, 0xBE, 0x00, 0x73, 0xFA, 0x80, 0x57, 0x6A, 0x14, 0x5E, 0xED, 
            0xC4, 0x8B, 0x74, 0x32, 0x87, 0x07, 0x93, 0xC8, 0xFC, 0xA6, 0xD8, 0x3E, 0x09, 0x6E, 0xC5, 0xF2, 
            0xA9, 0xC4, 0x21, 0xE7, 0x48, 0xB3, 0x73, 0x40, 0x5B, 0xE2, 0xFA, 0x8A, 0xE1, 0x58, 0x78, 0xE9, 
            0xD5, 0x23, 0x88, 0x75, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x52, 0x6F, 0x6F, 0x74, 0x2D, 0x43, 0x41, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x31, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x01, 0x43, 0x50, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x34, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0xF1, 0xB8, 0xA0, 0x64, 0xC1, 0x6D, 0xF3, 0x83, 0x29, 0x55, 0xC3, 0x29, 
            0x5B, 0x72, 0xF0, 0x33, 0x2E, 0x97, 0xEF, 0x14, 0x84, 0x8A, 0x68, 0x04, 0x9C, 0xA6, 0x8E, 0xAC, 
            0xDE, 0x14, 0x50, 0x33, 0xB8, 0x6C, 0x10, 0x8D, 0x48, 0x33, 0x5C, 0x5D, 0x0C, 0xAB, 0x77, 0x04, 
            0x62, 0x54, 0x47, 0x55, 0x45, 0x2A, 0x90, 0x00, 0x70, 0xB1, 0x56, 0x92, 0x5C, 0x17, 0x86, 0xE2, 
            0xCD, 0x20, 0x6D, 0xCC, 0xDC, 0x2C, 0x2E, 0x37, 0x6E, 0x27, 0xFC, 0xB4, 0x20, 0x66, 0xCC, 0x0A, 
            0x8C, 0xE9, 0xFE, 0xE8, 0x57, 0x04, 0xE6, 0xCA, 0x63, 0x1A, 0x2E, 0x7E, 0x91, 0x7E, 0x94, 0x7C, 
            0x39, 0x91, 0x77, 0x36, 0x29, 0xD1, 0x55, 0x61, 0x85, 0xBB, 0xD7, 0xB7, 0x73, 0xCA, 0x37, 0x47, 
            0x9E, 0x5F, 0xAA, 0xA3, 0xB6, 0x05, 0xE0, 0x01, 0xE1, 0xAC, 0xE5, 0x8D, 0xD8, 0xF8, 0x47, 0x82, 
            0xD6, 0x45, 0xFC, 0xE3, 0xA1, 0xCD, 0x03, 0xAB, 0x36, 0xF0, 0xF3, 0x86, 0xB1, 0xA2, 0xD1, 0x37, 
            0x40, 0xA1, 0x94, 0x8A, 0x53, 0xBA, 0x1B, 0x0D, 0x8C, 0x48, 0x63, 0xCD, 0x6B, 0x2C, 0x2E, 0x20, 
            0x64, 0x94, 0x80, 0x4C, 0x62, 0xFA, 0xA9, 0x3A, 0x7E, 0x33, 0xA9, 0xEA, 0x78, 0x6B, 0x59, 0xCA, 
            0xE3, 0xAB, 0x36, 0x45, 0xF4, 0xCB, 0x8F, 0xD7, 0x90, 0x6B, 0x82, 0x68, 0xCD, 0xAC, 0xF1, 0x7B, 
            0x3A, 0xEC, 0x46, 0x83, 0x1B, 0x91, 0xF6, 0xDE, 0x18, 0x61, 0x83, 0xBC, 0x4B, 0x32, 0x67, 0x93, 
            0xC7, 0x2E, 0x50, 0xD9, 0x1E, 0x36, 0xA0, 0xDC, 0xE2, 0xB9, 0x7D, 0xA0, 0x21, 0x3E, 0x46, 0x96, 
            0x02, 0x1F, 0x33, 0x1C, 0xBE, 0xAE, 0x8D, 0xFC, 0x92, 0x87, 0x32, 0xAA, 0x44, 0xDC, 0x78, 0xE7, 
            0x19, 0x9A, 0x3D, 0xDD, 0x57, 0x22, 0x7E, 0x9E, 0x77, 0xDE, 0x32, 0x63, 0x86, 0x93, 0x6C, 0x11, 
            0xAC, 0xA7, 0x0F, 0x81, 0x19, 0xD3, 0x3A, 0x99, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
        } ;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "NUSD - " + version + " - WB3000";
        }

        // Load from TMD
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog opentmd = new OpenFileDialog();
            opentmd.Filter = "TMD Files|tmd";
            opentmd.Title = "Open TMD";
            if (opentmd.ShowDialog() != DialogResult.Cancel)
            {
                // Read the tmd as a stream...
                FileStream fs = File.OpenRead(opentmd.FileName);
                byte[] tmd = ReadFully(fs, 20);
                WriteStatus("TMD Loaded (" + tmd.Length + " bytes)");

                // Read ID...
                titleidbox.Text = "";
                for (int x = 396; x < 404; x++)
                {
                    titleidbox.Text += MakeProperLength(ConvertToHex(Convert.ToString(tmd[x])));
                }
                WriteStatus("Title ID: " + titleidbox.Text);
                ReadIDType(titleidbox.Text);

                // Read Title Version...
                string tmdversion = "";
                for (int x = 476; x < 478; x++)
                {
                    //tmdversion += TrimLeadingZeros(Convert.ToString(tmd[x]));
                    tmdversion += MakeProperLength(ConvertToHex(Convert.ToString(tmd[x])));
                }
                titleversion.Text = Convert.ToString(int.Parse(tmdversion, System.Globalization.NumberStyles.HexNumber));

                // Read Content #...
                string contentstrnum = "";
                for (int x = 478; x < 480; x++)
                {
                    //contentstrnum += MakeProperLength(ConvertToHex(Convert.ToString(tmd[x])));
                    contentstrnum += TrimLeadingZeros(Convert.ToString(tmd[x]));
                }
                WriteStatus("Content Count: " + contentstrnum);

                string[] tmdcontents = GetContentNames(tmd, Convert.ToInt32(contentstrnum));
                string[] tmdsizes = GetContentSizes(tmd, Convert.ToInt32(contentstrnum));
                string[] tmdhashes = GetContentHashes(tmd, Convert.ToInt32(contentstrnum));

                for (int i = 0; i < Convert.ToInt32(contentstrnum); i++)
                {
                    WriteStatus("Content " + (i + 1) + ": " + tmdcontents[i] + " (" + Convert.ToString(int.Parse(tmdsizes[i], System.Globalization.NumberStyles.HexNumber)) + " bytes)");
                    //WriteStatus("  - Size: " + Convert.ToString(int.Parse(tmdsizes[i], System.Globalization.NumberStyles.HexNumber)) + " bytes");
                    //WriteStatus("  - Hash: " + tmdhashes[i]);

                }
            }
        }

        private void WriteStatus(string Update)
        {
            if (statusbox.Text == "")
                statusbox.Text = Update;
            else
                statusbox.Text += "\r\n" + Update;

            // MTP
            Console.WriteLine(Update);

            // Scroll to end of text box.
            statusbox.SelectionStart = statusbox.TextLength;
            statusbox.ScrollToCaret();
        }

        /// <summary>
        /// Reads data from a stream until the end is reached. The
        /// data is returned as a byte array. An IOException is
        /// thrown if any of the underlying IO calls fail.
        /// </summary>
        /// <param name="stream">The stream to read data from</param>
        /// <param name="initialLength">The initial buffer length</param>
        public static byte[] ReadFully(Stream stream, int initialLength)
        {
            // If we've been passed an unhelpful initial length, just
            // use 32K.
            if (initialLength < 1)
            {
                initialLength = 32768;
            }

            byte[] buffer = new byte[initialLength];
            int read = 0;

            int chunk;
            while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
            {
                read += chunk;

                // If we've reached the end of our buffer, check to see if there's
                // any more information
                if (read == buffer.Length)
                {
                    int nextByte = stream.ReadByte();

                    // End of stream? If so, we're done
                    if (nextByte == -1)
                    {
                        return buffer;
                    }

                    // Nope. Resize the buffer, put in the byte we've just
                    // read, and continue
                    byte[] newBuffer = new byte[buffer.Length * 2];
                    Array.Copy(buffer, newBuffer, buffer.Length);
                    newBuffer[read] = (byte)nextByte;
                    buffer = newBuffer;
                    read++;
                }
            }
            // Buffer is now too big. Shrink it.
            byte[] ret = new byte[read];
            Array.Copy(buffer, ret, read);
            return ret;
        }

        private string MakeProperLength(string hex)
        {
            if (hex.Length == 1)
                hex = "0" + hex;

            return hex;
        }

        private string ConvertToHex(string decval)
        {
            //Console.Write("Decimal Value: " + decval); // mtp

            // Convert text string to unsigned integer
            int uiDecimal = System.Convert.ToInt32(decval);

            string hexval;
            // Format unsigned integer value to hex and show in another textbox
            // Console.Write("Hex Value: " + String.Format("{0:x2}", uiDecimal)); // mtp
            hexval = String.Format("{0:x2}", uiDecimal);
            return hexval;
        }

        private void ReadIDType(string ttlid)
        {
            /*  Wiibrew TitleID Info...
                # 3 00000001: Essential system titles
                # 4 00010000 and 00010004 : Disc-based games
                # 5 00010001: Downloaded channels

                    * 5.1 000010001-Cxxx : Commodore 64 Games
                    * 5.2 000010001-Exxx : NeoGeo Games
                    * 5.3 000010001-Fxxx : NES Games
                    * 5.4 000010001-Hxxx : Channels
                    * 5.5 000010001-Jxxx : SNES Games
                    * 5.6 000010001-Nxxx : Nintendo 64 Games
                    * 5.7 000010001-Wxxx : WiiWare

                # 6 00010002: System channels
                # 7 00010004: Game channels and games that use them
                # 8 00010005: Downloaded Game Content
                # 9 00010008: "Hidden" channels
             */

            if (ttlid.Substring(0, 8) == "00000001")
            {
                WriteStatus("ID Type: System Title. BE CAREFUL!");
            }
            else if ((ttlid.Substring(0, 8) == "00010000") || (ttlid.Substring(0, 8) == "00010004"))
            {
                WriteStatus("ID Type: Disc-Based Game. Unlikely NUS Content!");
            }
            else if (ttlid.Substring(0, 8) == "00010001")
            {
                WriteStatus("ID Type: Downloaded Channel. Unlikely NUS Content!");
            }
            else if (ttlid.Substring(0, 8) == "00010002")
            {
                WriteStatus("ID Type: System Channel. BE CAREFUL!");
            }
            else if (ttlid.Substring(0, 8) == "00010004")
            {
                WriteStatus("ID Type: Game Channel. Unlikely NUS Content!");
            }
            else if (ttlid.Substring(0, 8) == "00010005")
            {
                WriteStatus("ID Type: Downloaded Game Content. Unlikely NUS Content!");
            }
            else if (ttlid.Substring(0, 8) == "00010008")
            {
                WriteStatus("ID Type: 'Hidden' Channel. Unlikely NUS Content!");
            }
            else
            {
                WriteStatus("ID Type: Unknown. Unlikely NUS Content!");
            }
        }

        private string TrimLeadingZeros(string num)
        {
            int startindex = 0;
            for (int i = 0; i < num.Length; i++)
            {
                if ((num[i] == 0) || (num[i] == '0'))
                    startindex += 1;
                else
                    break;
            }

            return num.Substring(startindex, (num.Length - startindex));
        }

        private string[] GetContentNames(byte[] tmdfile, int length)
        { 
            string[] contentnames = new string[length];
            int startoffset = 484;

            for (int i = 0; i < length; i++)
            {
                contentnames[i] = MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset]))) + MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset + 1]))) + MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset + 2]))) + MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset + 3])));
                startoffset += 36;
            }

            return contentnames;
        }

        private string[] GetContentSizes(byte[] tmdfile, int length)
        {
            string[] contentsizes = new string[length];
            int startoffset = 492;

            for (int i = 0; i < length; i++)
            {
                contentsizes[i] = MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset]))) + 
                    MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset + 1]))) + 
                    MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset + 2]))) + 
                    MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset + 3]))) + 
                    MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset + 4]))) + 
                    MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset + 5]))) + 
                    MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset + 6]))) + 
                    MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset + 7])));
                contentsizes[i] = TrimLeadingZeros(contentsizes[i]);
                /*contentsizes[i] = Convert.ToString(tmdfile[startoffset]) +
                    Convert.ToString(tmdfile[startoffset + 1]) +
                    Convert.ToString(tmdfile[startoffset + 2]) +
                    Convert.ToString(tmdfile[startoffset + 3]) +
                    Convert.ToString(tmdfile[startoffset + 4]) +
                    Convert.ToString(tmdfile[startoffset + 5]) +
                    Convert.ToString(tmdfile[startoffset + 6]) +
                    Convert.ToString(tmdfile[startoffset + 7]);
                contentsizes[i] = TrimLeadingZeros(contentsizes[i]);  */
                startoffset += 36;
            }

            return contentsizes;
        }

        private string[] GetContentHashes(byte[] tmdfile, int length)
        {
            string[] contenthashes = new string[length];
            int startoffset = 500;

            for (int i = 0; i < length; i++)
            {
                contenthashes[i] = MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset]))) + 
                    MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset + 1]))) + 
                    MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset + 2]))) + 
                    MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset + 3]))) + 
                    MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset + 4]))) + 
                    MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset + 5]))) + 
                    MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset + 6]))) + 
                    MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset + 7]))) +
                    MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset + 8]))) + 
                    MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset + 9]))) + 
                    MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset + 10]))) + 
                    MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset + 11]))) + 
                    MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset + 12]))) + 
                    MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset + 13]))) + 
                    MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset + 14]))) + 
                    MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset + 15]))) + 
                    MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset + 16]))) + 
                    MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset + 17]))) + 
                    MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset + 18]))) + 
                    MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset + 19])));
                //contentsizes[i] = TrimLeadingZeros(contentsizes[i]);
                startoffset += 36;
            }

            return contenthashes;
        }

        // Start NUS Download
        private void button3_Click(object sender, EventArgs e)
        {
            // NUSDownloader.RunWorkerAsync();

            // Test download a System Menu v4.0U
            //titleidbox.Text = "0000000100000002";
            //titleversion.Text = "417";
            //NUSDownloader_DoWork();
        }

        //private void NUSDownloader_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        public void NUSDownloader_DoWork()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            WriteStatus("Starting NUS Download. Please be patient!");
            button3.Enabled = false;
            titleidbox.Enabled = false;
            titleversion.Enabled = false;
            button3.Text = "Starting NUS Download!";

            CreateTitleDirectory();

            // Wii / DSi
            bool wiimode = radioButton1Wii.Checked;

            // Prevent crossthread issues
            string titleid = titleidbox.Text;

            string currentdir = Application.StartupPath;
            if (!(currentdir.EndsWith(@"\")) || !(currentdir.EndsWith(@"/")))
                currentdir += @"\";

            button3.Text = "Prerequisites: (0/2)";

            // Download TMD before the rest...
            string tmdfull = "tmd";
            if (titleversion.Text != "")
            {
                tmdfull += "." + titleversion.Text;
                Console.WriteLine("TitleVersion is specified. Retrieving version {0}", titleversion.Text);
            }
            else
            {
                Console.WriteLine("TitleVersion is blank. Retrieving the latest version...");
            }
            try
                {
            DownloadNUSFile(titleid, tmdfull, currentdir + titleid + @"\", 0, wiimode);
                }
            catch (Exception)
                {
                WriteStatus("NUS (404): tmd");
                button3.Enabled = true;
                titleidbox.Enabled = true;
                button3.Text = "Start NUS Download!";
                dlprogress.Value = 0;
                DeleteTitleDirectory();
                return;
                }
            button3.Text = "Prerequisites: (1/2)";
            dlprogress.Value = 50;

            // Download CETK before the rest...
                try
            {
            DownloadNUSFile(titleid, "cetk", currentdir + titleid + @"\", 0, wiimode);
                    }
            catch (Exception)
            {
                WriteStatus("NUS (404): cetk");
                button3.Enabled = true;
                titleidbox.Enabled = true;
                button3.Text = "Start NUS Download!";
                dlprogress.Value = 0;
                DeleteTitleDirectory();
                return;
            }
            button3.Text = "Prerequisites: (2/2)";
            dlprogress.Value = 100;

            // Read the tmd as a stream...
            FileStream fs = File.OpenRead(currentdir + titleid + @"\" + tmdfull);
            byte[] tmd = ReadFully(fs, 20);
            WriteStatus("NUS TMD Downloaded - " + tmd.Length + " bytes");

            // Read Title Version...
            string tmdversion = "";
            for (int x = 476; x < 478; x++)
            {
                tmdversion += MakeProperLength(ConvertToHex(Convert.ToString(tmd[x])));
            }
            titleversion.Text = Convert.ToString(int.Parse(tmdversion, System.Globalization.NumberStyles.HexNumber));

            // Read Content #...
            string contentstrnum = "";
            for (int x = 478; x < 480; x++)
            {
                contentstrnum += TrimLeadingZeros(Convert.ToString(tmd[x]));
            }
            WriteStatus("Content #: " + contentstrnum);
            button3.Text = "Content: (0/" + contentstrnum + ")";
            dlprogress.Value = 0;

            // Gather information...
            string[] tmdcontents = GetContentNames(tmd, Convert.ToInt32(contentstrnum));
            string[] tmdsizes = GetContentSizes(tmd, Convert.ToInt32(contentstrnum));
            string[] tmdhashes = GetContentHashes(tmd, Convert.ToInt32(contentstrnum));
            
            // Progress bar total size tally info...
            float totalcontentsize = 0;
            float currentcontentlocation = 0;
            for (int i = 0; i < tmdsizes.Length; i++)
            {
                totalcontentsize += int.Parse(tmdsizes[i], System.Globalization.NumberStyles.HexNumber);
            }

            for (int i = 0; i < tmdcontents.Length; i++)
            {
                try
                {
                    if ((localuse.Checked) && (File.Exists(currentdir + titleid + @"\" + tmdcontents[i])))
                    {
                        WriteStatus("Leaving local " + tmdcontents[i] + ".");
                    }
                    else
                    {
                        DownloadNUSFile(titleid, tmdcontents[i], currentdir + titleid + @"\", int.Parse(tmdsizes[i], System.Globalization.NumberStyles.HexNumber), wiimode);
                    }
                }
                catch (Exception)
                {
                    WriteStatus("NUS (404): " + tmdcontents[i]);
                    button3.Enabled = true;
                    titleidbox.Enabled = true;
                    button3.Text = "Start NUS Download!";
                    dlprogress.Value = 0;
                    DeleteTitleDirectory();
                    return;
                }

                button3.Text = "Content: (" + (i+1) + @"/" + contentstrnum + ")";
                currentcontentlocation += int.Parse(tmdsizes[i], System.Globalization.NumberStyles.HexNumber);
                //WriteStatus("Currently: " + Convert.ToString(currentcontentlocation) + @"/" + Convert.ToString(totalcontentsize) + " - " + Convert.ToInt32(((currentcontentlocation / totalcontentsize) * 100)) + "% Done.");
                dlprogress.Value = Convert.ToInt32(((currentcontentlocation / totalcontentsize) * 100));
            }

            WriteStatus("NUS Download Finished.");

            if ((packbox.Checked == true) && (wiimode == true))
            {
                PackWAD(titleid, tmdfull, tmdcontents.Length, tmdcontents);
            }

            button3.Enabled = true;
            titleidbox.Enabled = true;
            titleversion.Enabled = true;
            button3.Text = "Start NUS Download!";
            dlprogress.Value = 0;

        }

        private void CreateTitleDirectory()
        {
            string currentdir = Application.StartupPath;

            if (!(currentdir.EndsWith(@"\")) || !(currentdir.EndsWith(@"/")))
                currentdir += @"\";

            if ((localuse.Checked) && (Directory.Exists(currentdir + titleidbox.Text + @"\")))
            {
                WriteStatus("Using Local Files");
            }
            else
            {
                if (Directory.Exists(currentdir + titleidbox.Text))
                    Directory.Delete(currentdir + titleidbox.Text, true);

                Directory.CreateDirectory(currentdir + titleidbox.Text);
            }
        }

        private void DeleteTitleDirectory()
        {
            string currentdir = Application.StartupPath;

            if (!(currentdir.EndsWith(@"\")) || !(currentdir.EndsWith(@"/")))
                currentdir += @"\";

            if (Directory.Exists(currentdir + titleidbox.Text))
                Directory.Delete(currentdir + titleidbox.Text, true);

            //Directory.CreateDirectory(currentdir + titleidbox.Text);
        }

        private void DownloadNUSFile(string titleid, string filename, string placementdir, int sizeinbytes, bool iswiititle)
        {
            string nusfileurl;
            if (iswiititle)
                nusfileurl = NUSURL + titleid + @"/" + filename;
            else
                nusfileurl = DSiNUSURL + titleid + @"/" + filename;
            
            WriteStatus("Grabbing " + filename + "...");

            if (sizeinbytes != 0)
                statusbox.Text += " (" + Convert.ToString(sizeinbytes) + " bytes)";

            
                generalWC.DownloadFile(nusfileurl, placementdir + filename);
            
            
        }

        public void PackWAD(string titleid, string tmdfilename, int contentcount, string[] contentnames)
        { 
            // Directory stuff
            string currentdir = Application.StartupPath;
            if (!(currentdir.EndsWith(@"\")) || !(currentdir.EndsWith(@"/")))
                currentdir += @"\";
                
            // Create ticket file holder
            FileStream fs1 = File.OpenRead(currentdir + titleid + @"\cetk");
            byte[] cetkbuf = ReadFully(fs1, 20);
            fs1.Close();

            // Create tmd file holder
            FileStream fs2 = File.OpenRead(currentdir + titleid + @"\" + tmdfilename);
            byte[] tmdbuf = ReadFully(fs2, 20);
            fs2.Close();

            //WriteStatus("Ticket and TMD loaded into memory...");

            // Create wad file
            FileStream wadfs = new FileStream(currentdir + titleid + @"\" + titleid + ".wad", FileMode.Create);

            // Add wad stuffs
            WADHeader wad = new WADHeader();
            wad.HeaderSize = 0x20;
            wad.WadType = 0x49730000;
            wad.CertChainSize = 0xA00;

            // TMDSize is length of buffer.
            wad.TMDSize = tmdbuf.Length;
            // TicketSize is length of cetkbuf.
            wad.TicketSize = cetkbuf.Length;

            // Write cert[] to 0x40.
            wadfs.Seek(0x40, SeekOrigin.Begin);
            wadfs.Write(cert, 0, cert.Length);

            WriteStatus("Cert wrote at 0x40");

            // Need 64 byte boundary...
            wadfs.Seek(2624, SeekOrigin.Begin);

            // Cert is 2560
            // Write ticket at this point...
            wad.TicketSize = 0x2A4;
            wadfs.Write(cetkbuf, 0, wad.TicketSize);

            WriteStatus("Ticket wrote at " + (wadfs.Length - 0x2A4));

            // Need 64 byte boundary...
            wadfs.Seek(ByteBoundary(Convert.ToInt32(wadfs.Length)), SeekOrigin.Begin);

            // Write TMD at this point...
            wadfs.Write(tmdbuf, 0, 484 + (contentcount*36));

            WriteStatus("TMD wrote at " + (wadfs.Length - (484 + (contentcount * 36))));

            // Preliminary data size of wad file.
            wad.DataSize = 0;

            // Loop n Add contents
            for (int i = 0; i < contentcount; i++)
            {
                // Need 64 byte boundary...
                wadfs.Seek(ByteBoundary(Convert.ToInt32(wadfs.Length)), SeekOrigin.Begin);

                // Create content file holder
                FileStream cont = File.OpenRead(currentdir + titleid + @"\" + contentnames[i]);
                byte[] contbuf = ReadFully(cont, 20);
                cont.Close();

                wadfs.Write(contbuf, 0, contbuf.Length);

                WriteStatus(contentnames[i] + " wrote at " + (wadfs.Length - contbuf.Length));

                wad.DataSize += contbuf.Length;
            }

            wadfs.Seek(0, SeekOrigin.Begin);

            // Write initial part of header
            byte[] start = new byte[8] {0x00, 0x00, 0x00, 0x20, 0x49, 0x73, 0x00, 0x00};
            wadfs.Write(start, 0, start.Length);

            //WriteStatus("Initial header wrote");

            // Write CertChainLength
            wadfs.Seek(0x08, SeekOrigin.Begin);
            byte[] chainsize = InttoByteArray(wad.CertChainSize);
            wadfs.Write(chainsize, 0, 4);
            
            // Write res
            byte[] reserved = new byte[4] { 0x00, 0x00, 0x00, 0x00 };
            wadfs.Seek(0x0C, SeekOrigin.Begin);
            wadfs.Write(reserved, 0, 4);

            // Write ticketsize
            byte[] ticketsize = new byte[4] { 0x00, 0x00, 0x02, 0xA4 };
            wadfs.Seek(0x10, SeekOrigin.Begin);
            wadfs.Write(ticketsize, 0, 4);

            // Write tmdsize
            int strippedtmd = 484 + (contentcount * 36);
            byte[] tmdsize = InttoByteArray(strippedtmd);
            wadfs.Seek(0x14, SeekOrigin.Begin);
            wadfs.Write(tmdsize, 0, 4);

            // Write data size
            wadfs.Seek(0x18, SeekOrigin.Begin);
            byte[] datasize = InttoByteArray(wad.DataSize);
            wadfs.Write(datasize, 0, 4);

            WriteStatus("WAD Created: " + titleid + ".wad");
            wadfs.Close();
        }

        private long ByteBoundary(int currentlength)
        {
            // Gets the next 0x40 offset.
            long thelength = currentlength - 1;
            long remainder = 1;

            while (remainder != 0)
            {
                thelength += 1;
                remainder = thelength % 0x40;
            }

            //WriteStatus("Initial Size: " + currentlength);
            //WriteStatus("0x40 Size: " + thelength);

            return (long) thelength;
        }

        private byte[] InttoByteArray(int size)
        {
            byte[] b = new byte[4];
            b = BitConverter.GetBytes(size);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(b);
            
            return b;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2DS.Checked == true)
            { 
                // Cannot Pack WADs
                packbox.Checked = false;
                packbox.Enabled = false;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1Wii.Checked == true)
            { 
                // Can pack WADs
                //packbox.Checked = true;
                packbox.Enabled = true;
            }
        }

        // About box
        private void button2_Click(object sender, EventArgs e)
        {
            // Display About Text...
            statusbox.Text = "";
            WriteStatus("NUS Downloader (NUSD)");
            WriteStatus("You are running version: " + version);
            WriteStatus("This program coded by WB3000");
            WriteStatus("");
            WriteStatus("Special thanks to:");
            WriteStatus(" * Crediar for his wadmaker tool + source, and for the advice!");
            WriteStatus(" * SquidMan and Galaxy for advice/sources.");
            WriteStatus(" * #WiiBrew for general assistance whenever I had questions.");
        }

        private void InitializeComponent()
        {
            this.titleidbox = new System.Windows.Forms.TextBox();
            this.titleversion = new System.Windows.Forms.TextBox();
            this.statusbox = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.packbox = new System.Windows.Forms.CheckBox();
            this.localuse = new System.Windows.Forms.CheckBox();
            this.radioButton1Wii = new System.Windows.Forms.RadioButton();
            this.radioButton2DS = new System.Windows.Forms.RadioButton();
            this.dlprogress = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // titleidbox
            // 
            this.titleidbox.Location = new System.Drawing.Point(12, 39);
            this.titleidbox.Name = "titleidbox";
            this.titleidbox.Size = new System.Drawing.Size(224, 20);
            this.titleidbox.TabIndex = 0;
            // 
            // titleversion
            // 
            this.titleversion.Location = new System.Drawing.Point(263, 39);
            this.titleversion.Name = "titleversion";
            this.titleversion.Size = new System.Drawing.Size(109, 20);
            this.titleversion.TabIndex = 1;
            // 
            // statusbox
            // 
            this.statusbox.Location = new System.Drawing.Point(12, 144);
            this.statusbox.Name = "statusbox";
            this.statusbox.Size = new System.Drawing.Size(360, 299);
            this.statusbox.TabIndex = 2;
            this.statusbox.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(192, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(180, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Load From TMD";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(329, 449);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(43, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "?";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(13, 75);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(359, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "Start NUS Download";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // packbox
            // 
            this.packbox.AutoSize = true;
            this.packbox.Location = new System.Drawing.Point(183, 466);
            this.packbox.Name = "packbox";
            this.packbox.Size = new System.Drawing.Size(86, 17);
            this.packbox.TabIndex = 6;
            this.packbox.Text = "Pack->WAD";
            this.packbox.UseVisualStyleBackColor = true;
            // 
            // localuse
            // 
            this.localuse.AutoSize = true;
            this.localuse.Location = new System.Drawing.Point(183, 504);
            this.localuse.Name = "localuse";
            this.localuse.Size = new System.Drawing.Size(128, 17);
            this.localuse.TabIndex = 7;
            this.localuse.Text = "Use/Keep Local Files";
            this.localuse.UseVisualStyleBackColor = true;
            // 
            // radioButton1Wii
            // 
            this.radioButton1Wii.AutoSize = true;
            this.radioButton1Wii.Location = new System.Drawing.Point(34, 465);
            this.radioButton1Wii.Name = "radioButton1Wii";
            this.radioButton1Wii.Size = new System.Drawing.Size(40, 17);
            this.radioButton1Wii.TabIndex = 8;
            this.radioButton1Wii.TabStop = true;
            this.radioButton1Wii.Text = "Wii";
            this.radioButton1Wii.UseVisualStyleBackColor = true;
            this.radioButton1Wii.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // radioButton2DS
            // 
            this.radioButton2DS.AutoSize = true;
            this.radioButton2DS.Location = new System.Drawing.Point(34, 504);
            this.radioButton2DS.Name = "radioButton2DS";
            this.radioButton2DS.Size = new System.Drawing.Size(40, 17);
            this.radioButton2DS.TabIndex = 9;
            this.radioButton2DS.TabStop = true;
            this.radioButton2DS.Text = "DS";
            this.radioButton2DS.UseVisualStyleBackColor = true;
            this.radioButton2DS.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // dlprogress
            // 
            this.dlprogress.Location = new System.Drawing.Point(13, 115);
            this.dlprogress.Name = "dlprogress";
            this.dlprogress.Size = new System.Drawing.Size(359, 23);
            this.dlprogress.TabIndex = 10;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(392, 533);
            this.Controls.Add(this.dlprogress);
            this.Controls.Add(this.radioButton2DS);
            this.Controls.Add(this.radioButton1Wii);
            this.Controls.Add(this.localuse);
            this.Controls.Add(this.packbox);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.statusbox);
            this.Controls.Add(this.titleversion);
            this.Controls.Add(this.titleidbox);
            this.Name = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}


public class NUSDownloaderClass
{
    public static void Main(string[] args)
    {
        NUS_Downloader.Form1 myForm = new NUS_Downloader.Form1();

        // Initialize the checkboxes and radio boxes
        myForm.packbox.Checked = true;
        myForm.localuse.Checked = true;
        myForm.radioButton1Wii.Checked = true;
        myForm.radioButton2DS.Checked = false;

        Console.WriteLine("NUS Downloader Command Line v0.2 by wiiNinja. Original GUI code by WB3000");
        if (args.Length < 2)
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("    nusd <titleID> <titleVersion | *> [packwad] [localuse]");
            Console.WriteLine("\nWhere:");
            Console.WriteLine("    titleID = The ID of the title to be downloaded");
            Console.WriteLine("    titleVersion = The version of the title to be downloaded");
            Console.WriteLine("              Use \"*\" (no quotes) to get the latest version");
            Console.WriteLine("    packwad = Optional: A wad file will be generated");
            Console.WriteLine("    localuse = Optional: All the downloaded files will be retained locally");
        }
        else
        {
            for (int i = 0; i < args.Length; i++)
            {
                Console.WriteLine("{0}", args[i]);
                switch (i)
                {
                    case 0:
                        // First command line argument is ALWAYS the TitleID
                        myForm.titleidbox.Text = args[i];
                        break;

                    case 1:
                        // Second command line argument is ALWAYS the TitleVersion. 
                        // User may specify a "*" to retrieve the latest version
                        if (args[i] == "*")
                            myForm.titleversion.Text = "";
                        else
                            myForm.titleversion.Text = args[i];
                        break;

                    default:
                        // Only two other cmd line args are handled: packwad and localuse
                        if (args[i] == "packwad")
                            myForm.packbox.Checked = true;
                        else if (args[i] == "localuse")
                            myForm.localuse.Checked = true;
                        break;
                }
            }

            // Just a test download of the System Menu 4.0U
            //myForm.titleidbox.Text = "0000000100000002";
            //myForm.titleversion.Text = "417";
            //myForm.packbox.Checked = true;
            //myForm.localuse.Checked = true;
            //myForm.radioButton1Wii.Checked = true;
            //myForm.radioButton2DS.Checked = false;

            // Call to get the files from server
            myForm.NUSDownloader_DoWork();

            Console.WriteLine("\nSuccessfully downloaded the title {0} version {1}", args [0], args [1]);
        }
    }
}