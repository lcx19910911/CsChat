namespace CsChat.Respository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _222 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.esc_chat_record", "TimeStamp");
            DropColumn("dbo.esc_chat_record", "UpdatedTime");
            DropColumn("dbo.esc_chat_relation", "TimeStamp");
            DropColumn("dbo.esc_chat_relation", "UpdatedTime");
            DropColumn("dbo.esc_user", "TimeStamp");
            DropColumn("dbo.esc_chat_userDevice", "TimeStamp");
            DropColumn("dbo.esc_chat_userDevice", "UpdatedTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.esc_chat_userDevice", "UpdatedTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.esc_chat_userDevice", "TimeStamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.esc_user", "TimeStamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.esc_chat_relation", "UpdatedTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.esc_chat_relation", "TimeStamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.esc_chat_record", "UpdatedTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.esc_chat_record", "TimeStamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
    }
}
