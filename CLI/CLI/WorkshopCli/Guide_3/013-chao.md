[color=white]
Fantastico! Ja tens o fundo e o chao. Agora vamos desenhar
o teu skater!

No "love.load()" o codigo ja carregou a imagem do skater e
preparou 3 poses diferentes a usar "recortes" (Quads):

- poseAndar - quando o skater esta a andar
- poseSaltar - quando o skater esta no ar
- poseAgachar - quando o skater se agacha

Isto funciona porque a imagem "skater.png" tem 3 desenhos
lado a lado. O computador recorta a parte certa para cada pose.

Agora vamos desenhar o skater! Adiciona o seguinte no
"love.draw()", DEPOIS do chao:

[/color] [color=blue]
    -- Pose do jogador
    local poseAtual = poseAndar

    -- Jogador
    love.graphics.setColor(1, 1, 1)
    love.graphics.draw(imagemSkater, poseAtual,
        jogador.x, jogador.y,
        0, escalaSkater, escalaSkater
    )
[/color] [color=white]

Usamos "poseAndar" por agora. Mais tarde vamos mudar a
pose conforme o jogador salta ou se agacha.

O "setColor(1, 1, 1)" volta a cor para branco (sem filtro).

Executa o jogo (Alt + L) para veres o skater no ecra!
[/color]

