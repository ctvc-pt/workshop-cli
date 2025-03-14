[color=white]
Já tens o personagem do jogo, o próximo passo é permitir que ela 
possa cair.

3. Na nossa função "update()" adiciona o seguinte código:
[/color] [color=blue]
    bird.speed = bird.speed + bird.gravity
    bird.y = bird.y + bird.speed
[/color] [color=white]
Executa o jogo (Alt + L), O pássaro começa a cair sozinho? 
Isso acontece porque ele está a ser puxado para baixo pela gravidade!
[/color] [color=red]
     ____  _____ ____    _    _____ ___ ___  
    |  _ \| ____/ ___|  / \  |  ___|_ _/ _ \
    | | | |  _| \___ \ / _ \ | |_   | | | | |
    | |_| | |___ ___) / ___ \|  _|  | | |_| |
    |____/|_____|____/_/   \_\_|   |___\___/
[/color] [color=white]

Boa! Aumenta o valor da gravidade (gravity = 0.25) para 0.5. O que acontece?.

Agora no proximo passo vamos fazer com que o pássaro possa voar.
[/color] 