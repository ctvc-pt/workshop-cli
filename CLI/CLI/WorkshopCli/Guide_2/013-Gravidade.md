[color=white]
Já tens o personagem do jogo, o próximo passo é permitir que ele
possa cair.

3. Na nossa função "update()" adiciona o seguinte código:
[/color] [color=blue]
   passaro.velocidade = passaro.velocidade + passaro.gravidade
   passaro.y = passaro.y + passaro.velocidade
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

Aumenta o valor da gravidade.

Agora no proximo passo vamos fazer com que o pássaro possa voar.
[/color] 