using System;
using UnityEngine;

<<<<<<< Updated upstream
public class Readme : ScriptableObject
{
    public Texture2D icon;
    public float iconMaxWidth = 128f;
    public string title;
    public string titlesub;
    public Section[] sections;
    public bool loadedLayout;

    [Serializable]
    public class Section
    {
        public string heading, text, linkText, url;
    }
}
=======
public class Readme : ScriptableObject {
	public Texture2D icon;
	public string title;
	public Section[] sections;
	public bool loadedLayout;
	
	[Serializable]
	public class Section {
		public string heading, text, linkText, url;
	}
}
>>>>>>> Stashed changes
