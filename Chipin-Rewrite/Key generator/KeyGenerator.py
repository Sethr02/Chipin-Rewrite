import secrets

def generate_hex_string(length):
    return secrets.token_hex(length // 2 + 1)[:length]

hex_string = generate_hex_string(19)
print(hex_string)
