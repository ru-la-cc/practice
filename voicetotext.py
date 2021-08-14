# -*- coding: utf-8 -*-

from pydub import AudioSegment
from pydub.silence import split_on_silence
import speech_recognition as sr

VOICE_FILE = "音声ファイル.wav"
TEXT_FILE = "音声テキスト.txt"

# wavファイルのデータ取得
sound = AudioSegment.from_file(VOICE_FILE, format="wav")
r = sr.Recognizer()

# wavデータの分割（無音部分で区切る）
# min_silence_len : 何ミリ秒以上無音なら分割するか
# silence_thresh : 何dBFS以下で無音と判断するか（負数で指定）
# keep_silence : 分割後に残す無音時間（ミリ秒）
chunks = split_on_silence(sound, min_silence_len=1000, silence_thresh=-20, keep_silence=500, seek_step=6000)

print(f"音声ファイル -> {VOICE_FILE}")
print(f"出力ファイル -> {TEXT_FILE}")

print("音声認識 start")
fs = open(TEXT_FILE, mode="w")
# 分割したデータ毎にファイルに出力
for i, chunk in enumerate(chunks):
    parse_file = f"{i+1:003d}.wav"
    chunk.export(parse_file, format="wav")
    with sr.AudioFile(parse_file) as src:
        audio = r.record(src)
    try:
        print(f"解析中... {i+1}/{len(chunks)}")
        fs.write(f"-----> {parse_file}\n")
        text = str(r.recognize_google(audio, language='ja-JP')).replace(" ", "\n")
        fs.write(text + "\n")
    except sr.UnknownValueError:
        print(f"{parse_file} : 聞き取れません")
        fs.write(" <<< よくわかりません >>>\n")
    except sr.RequestError as e:
        print(f"{parse_file} : 無反応 ... {e}")
        fs.write(" <<< 変換できません >>>\n")
    fs.flush()
    
fs.close()
print("音声認識 end")
