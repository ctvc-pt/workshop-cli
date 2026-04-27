[color=white]
O jogo esta a funcionar mas esta silencioso. Vamos meter musica
de fundo a tocar enquanto jogas!

DENTRO da funcao "love.load()", no fim, adiciona:

[/color] [color=blue]
    -- Musica de fundo
    musicaFundo = love.audio.newSource("musicaFundo.ogg", "stream")
    musicaFundo:setLooping(true)
    musicaFundo:setVolume(0.4)
    musicaFundo:play()
[/color] [color=white]

Como funciona:
- "newSource" carrega o ficheiro de musica.
- "stream" e usado para musicas longas (mais leve que "static").
- "setLooping(true)" faz a musica repetir sem parar.
- "setVolume(0.4)" baixa o volume para 40% — para nao tapar os
  outros sons que vamos adicionar a seguir.
- "play()" comeca a tocar logo no inicio do jogo.

Executa o jogo (Alt + L). Vais ouvir a musica a tocar em loop!
[/color]
