let kinjo;
let yoshinoya;
let taremaku;
let hyakugoju;
let y150;
let tokumori;
let sonoseki;
let u_noji;
let suwareta;
let oomoritsuyu;
let tsuyudaku;
let kuitai;
let negidaku;
let oomorinegi;
let tanomikata;
let negioome;
let nikusukuname;
let oomorigyoku;
let korewotanomu;
let teninmark;
let gyusyake;


$(function(){
	$("#mkyoshi").on("click", function(){
		console.log($("input[name='kinjo']").val());
		kinjo = $("input[name='kinjo']").val();
		yoshinoya = $("input[name='yoshinoya']").val();
		taremaku = $("input[name='taremaku']").val();
		hyakugoju = $("input[name='hyakugoju']").val();
		y150 = $("input[name='y150']").val();
		tokumori = $("input[name='tokumori']").val();
		sonoseki = $("input[name='sonoseki']").val();
		u_noji = $("input[name='u_noji']").val();
		suwareta = $("input[name='suwareta']").val();
		oomoritsuyu = $("input[name='oomoritsuyu']").val();
		tsuyudaku = $("input[name='tsuyudaku']").val();
		kuitai = $("input[name='kuitai']").val();
		negidaku = $("input[name='negidaku']").val();
		oomorinegi = $("input[name='oomorinegi']").val();
		tanomikata = $("input[name='tanomikata']").val();
		negioome = $("input[name='negioome']").val();
		nikusukuname = $("input[name='nikusukuname']").val();
		oomorigyoku = $("input[name='oomorigyoku']").val();
		korewotanomu = $("input[name='korewotanomu']").val();
		teninmark = $("input[name='teninmark']").val();
		gyusyake = $("input[name='gyusyake']").val();
		const YOSHINOYA_BASE =
				`昨日、${kinjo}の${yoshinoya}行ったんです。${yoshinoya}。<br>
				そしたらなんか人がめちゃくちゃいっぱいで座れないんです。<br>
				で、よく見たらなんか${taremaku}、${hyakugoju}、とか書いてあるんです。<br>
				もうね、アホかと。馬鹿かと。<br>
				お前らな、${hyakugoju}如きで普段来てない${yoshinoya}に来てんじゃねーよ、ボケが。<br>
				${y150}だよ、${y150}。<br>
				なんか親子連れとかもいるし。一家４人で${yoshinoya}か。おめでてーな。<br>
				よーしパパ${tokumori}ぞー、とか言ってるの。もう見てらんない。<br>
				お前らな、${y150}やるから${sonoseki}と。<br>
				${yoshinoya}ってのはな、もっと殺伐としてるべきなんだよ。<br>
				${u_noji}奴といつ喧嘩が始まってもおかしくない、<br>
				刺すか刺されるか、そんな雰囲気がいいんじゃねーか。女子供は、すっこんでろ。<br>
				で、やっと${suwareta}かと思ったら、隣の奴が、${oomoritsuyu}、とか言ってるんです。<br>
				そこでまたぶち切れですよ。<br>
				あのな、${tsuyudaku}なんてきょうび流行んねーんだよ。ボケが。<br>
				得意げな顔して何が、${tsuyudaku}で、だ。<br>
				お前は本当に${tsuyudaku}を${kuitai}のかと問いたい。問い詰めたい。小１時間問い詰めたい。<br>
				お前、${tsuyudaku}って言いたいだけちゃうんかと。<br>
				${yoshinoya}通の俺から言わせてもらえば今、${yoshinoya}通の間での最新流行はやっぱり、<br>
				${negidaku}、これだね。<br>
				${oomorinegi}。これが通の${tanomikata}。<br>
				${negidaku}ってのは${negioome}。そん代わり${nikusukuname}。これ。<br>
				で、それに${oomorigyoku}。これ最強。<br>
				しかし${korewotanomu}と次から${teninmark}という危険も伴う、諸刃の剣。<br>
				素人にはお薦め出来ない。<br>
				まあお前らド素人は、${gyusyake}ってこった。`;
			$("#copipe").html(YOSHINOYA_BASE);
	});
});