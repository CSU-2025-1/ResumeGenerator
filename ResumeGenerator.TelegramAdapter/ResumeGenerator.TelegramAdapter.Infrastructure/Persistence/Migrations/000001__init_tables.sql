CREATE TABLE IF NOT EXISTS telegram_chat
(
    user_id UUID   NOT NULL UNIQUE,
    ext_id  BIGINT NOT NULL UNIQUE,
    PRIMARY KEY (user_id, ext_id)
);