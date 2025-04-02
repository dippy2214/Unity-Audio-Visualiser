# Unity Audio Visualiser 🎶
This project initially began as a side project being developed for my sister, who has always enjoyed music with visualiser effects.
working towards the deadline of her birthday, which gave me roughly a month for this project, I had a great time figuring out how to
make use of fast forier transforms and import files in unity for the creation of this program.

The key goals I was working towards with this project were to make it look good (not necessarily complex, just simple and clean), to
make it capable of loading external files in some way, and to make it within the one month deadline. I am pleased to say that all of 
these have been achieved, and I was even able to add some simple visual customisation options before shipping.

## How to Use 📚
The audio visualiser upon being ran will look for a folder in your documents titled "AudioVisualiserMusic". It will look for the audio
you have placed in this folder with the .mp3 file extension, and if the file doesn't exist it will create it. simply put your music 
into this folder and enjoy listening!

## Demo Video ⏯️
<a href="https://www.youtube.com/shorts/TcbWR1ghD74">
  <img src="https://i.ytimg.com/vi/TcbWR1ghD74/oar2.jpg" alt="Watch the video" width="255" height="400">
</a>

## The Development Process 🛠️ 

### 🤔 Defining The Goals
My first step with this project was actually talking to my sister a bit about what she would be interested in. Having these chats and
defining exactly what was required to make the product that she would enjoy was my first time making a project for somebody other than 
myself outside of uni, which was an interesting experience despite obviously being quite unofficial. These experiences were built upon
further through things like my third year professional project, where we worked with industry clients, and I found the lessons learned 
here to come in handy with that project too, espoecially the importance of defining proper goals and end points for the project. 

The first point key point that was brought up was that she wanted to be able to use this with her own music and load in her own songs.
Initially I wanted to do more than just this, as I was hoping to give the program settings to visualise all system audio so that music
didn't necessarily need to be played through the visualiser program, and make it easier than it ended up being to load in files, but this
proved to be a lesson for me in managing scope. I had given myself a month for a project I wasn't working on full time to figure out how
to make systems I had no experience with, and these features had to be cut in the interests of finishing the project for her birthday.

The other point to note was that we wanted it to look nice, since that was the whole point of having the audio visualised in the first place.

### 💡 Starting The Research
For me the next step of this project had to be research. Like many of my other projects, I had no idea what I was doing to begin with. I
looked for resources from people with experience around the internet on how audio visualisers worked, coming across mathematical concepts
like fast forier transforms (a formula for breaking down a given wave or signal into the individual frequencies that make it up and how strong
they are). By applying fast forier transforms to the sound wave our music is generating, audio visualisers will break the sound down into 
frequency bands. Each band will be a different size, often based on how our ears percieve sounds. Generally speaking lower frequencies will
have thinner bands (20-60 hertz for example) whereas higher frequencies will have wider bands (6000 - 20000 hertz), because our ears percieve
these lower frequency sounds with more precision than the higher frequency ones, and as such lower frequencies and bass lines will also be a
more significant part of most soundtracks. Breaking down the audio into these bands is important because we need to break down continuous 
frequency into single numbers we can use.

Once we have these bands, making the visualiser is just a matter of applying the data on how strongly each band is represented in the audio
to some kind of visual feedback to create nice looking effects, which for me was a fine task since this kind of real time representation
of data made use of a lot of transferable skills of mine from game development (especially since I had chosen to use unity).

For anybody interested in pursuing a similar project, one of the most useful sources for me was [this video series](https://www.youtube.com/playlist?list=PL3POsQzaCw53p2tA6AWf7_AWgplskR0Vo)
by Peer Play, which goes over the process of making an audio visualiser in unity and the kind of code you will need in the backend. I made
strong use of this in this project, and credit him for much of the code structures and ideas used. It was a very valuable learning resource 
for me.

### 👨‍💻 Starting Development
The first step I took in development was setting up a basic testing scene to experiment with. This entailed a simple setup with an audio
source and a random copyright free song I found on the internet (pure coincidence that I was testing with an 8 bit rickroll I assure you),
plus an empty gameobject with the script I wanted to create the visualisation with.

The script would create a bunch of cubes , which at first I would change the scale of to see how my audio bands were working. My code didn't 
yet break the data down into bands, but instead used 512 samples, which represent exact frequencies I was taking data from (we need to take
data from specific points at some stage to get a number from the fast forier transforms). To start with I just tested these samples, which 
were evenly spaced through the frequency range of the audio file, and this produced some good (albeit not quite pretty yet) results.

<a>
  <img src="https://github.com/user-attachments/assets/16b526d0-fa2b-4f39-a6e4-b4e030c94065" alt="Sample Visualisation" width="400" height="255">
</a>

As we expected, the height is weighted heavily towards one end. This is because of the way that the lower frequency sounds appear so much
stronger in our audio, as we discussed earlier. The wave along the cubes is also not entirely smooth, often jittering a bit between individual
samples. The solution to this is the audio band system.

### 🤠 I Was In A Band
To set up the audio bands, I took a system shown in the Peer Play video series above:

        22050 / 512 = 43 hertz per sample
        20 - 60 hertz
        60 - 250 hertz
        250 - 500 hertz
        500 - 2000 hertz
        4000 - 6000 hertz
        6000 - 20000 hertz

        0 - 2 samples = 86 hertz
        1 - 4 samples = 172 hertz -> 87 - 258 hertz range
        2 - 8 samples = 344 hertz -> 259-602
        3 - 16
        4 - 32
        5 - 64
        6 - 128
        7 - 256

        510 total - add 2 to last one - 512 samples (nice power of 2)

We calculate some of the desired audio frequency ranges, and scale these to be a rough guide for the samples in each band. As you can see this
follows an exponential pattern, however you can change this depending on your specific use case and it is open to tweaking. There isn't a hard
and fast rule for these ranges as such, we are just looking for something that creates a nice visual effect.

Coding this in we have 8 bands to use in the visualiser, and after a bit of cleanup we get the following effect:

<a>
  <img src="https://github.com/user-attachments/assets/a7794e97-2e26-4a95-9d3c-c559675a5301" alt="Sample Visualisation" width="400" height="255">
</a>

(apologies for the low quality pictures of my laptop, these were the only pictures I thought to take through the development of this project, and 
since I didn't upload this project to github while making it the older versions have been lost).

Now that we have a working concept for the band system, it's time to expand.

### 📈 Expanding The Systems
After implementing the 8 bands, I quickly switched to allow myself to make use of 64 bands as well. This was entirely for aesthetic purposes, and
the bands are made following similar logic to the 8 bands, just divided up more. having 64 bands let me have 64 cubes, which let me play with the
visuals more to customise it how I wanted. I was going for a circular pattern, which will be more clear in the final design. However at this stage
I started experimenting with lots more looks for the program. I could easily extract data such as amplitude from the data, and I experimented for a
while with different looks. 

However, none of this was particularly technically impressive, so lets skip forward to some more features.

### 📂 Loading In The Data
Let's talk loading in the files, the second goal of this project. In short, when using unity, this sucks. It is not something the engine was 
designed for. Loading in files which are part of your project from the very beginning is one thing, but loading in files that are not present at
compile time is another completely. The files I want to interact with need to be external from the project, and for this we need some clever
workarounds.

Option 1 was to create my own file selecting system using the windows API myself, since unity doesn't have a tool for this. It seems like a fantastically
interesting project, but for the 1 month time frame I didn't think this was the best course of action. I could try to find someone else who had done
it already, but I went for another, sillier option. 

Unity allows me to generate web requests, and these can be used on the local computer. Using this, I can run a unity web request on the local device
to look for multimedia MPEG files, and then download these into the program. Is this performant? No. Is it a simple solution? Also no. But it was
quick, easy and just the right amount of silly for a project like this (and besides, realising I could use web requests this way did make me smile).

This is what I did, but my implementation comes with some real drawbacks. Currently the program loads all audio files on startup, which creates a
major lag spike. It also stores all audio files in memory for the entire time the program is running, making it memory inefficient. Both of these
problems also scale dramatically the more audio files you put in that folder to load into the program. The simple answer on how to fix this is to
only load up the audio files for each song, which would have fixed both of these at the expense of a smaller lag spike at the start of every track.
However, the only true solution would be to make a proper file loading system that didn't run through web requests, which is not a performant answer
to the problem.


