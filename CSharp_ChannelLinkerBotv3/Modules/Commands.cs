using ChannelLinkerBot.DTO;
using ChannelLinkerBotv2.DTO;
using Discord;using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using TemplateBot;

namespace ChannelLinkerBot.Modules
{
    public class Main : ModuleBase<SocketCommandContext>
    {


        [Command("linkmode")]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        public async Task linkmode([Optional]string linkID)
        {
            string message = "Something wrong happend!";

            if (linkID != null)
            {
                var ilinkID = int.Parse(linkID);
                var links = CommandHandler.ChannelsLinkedList.FindAll(x => x.GuildID == Context.Guild.Id);

                if (CommandHandler.ChannelsLinkedList.Exists(x => x.GuildID == Context.Guild.Id) && links.Count >= (ilinkID + 1))
                {
                    var setting = links[ilinkID];

                    if (setting.LinkMode == ChannelLinkDTO.Modes.TextOnly)
                    {
                        setting.LinkMode = ChannelLinkDTO.Modes.FilesOnly;
                        message = $@"Link mode for channellink { linkID} is set to 'Files only'";

                    }
                    else if (setting.LinkMode == ChannelLinkDTO.Modes.FilesOnly)
                    {
                        setting.LinkMode = ChannelLinkDTO.Modes.PicturesAndFilesOnly;
                        message = $@"Link mode for channellink { linkID} is set to 'Pictures and Files only'";
                    }
                    else if (setting.LinkMode == ChannelLinkDTO.Modes.PicturesAndFilesOnly)
                    {
                        setting.LinkMode = ChannelLinkDTO.Modes.EmbedOnly;
                        message = $@"Link mode for channellink { linkID} is set to 'Embed only'";
                    }
                    else if (setting.LinkMode == ChannelLinkDTO.Modes.EmbedOnly)
                    {
                        setting.LinkMode = ChannelLinkDTO.Modes.NoneEmbedOnly;
                        message = $@"Link mode for channellink { linkID} is set to 'None Embed only'";
                    }
                    else if (setting.LinkMode == ChannelLinkDTO.Modes.NoneEmbedOnly)
                    {
                        setting.LinkMode = ChannelLinkDTO.Modes.All;
                        message = $@"Link mode for channellink { linkID} is set to 'All'";
                    }
                    else
                    {
                        setting.LinkMode = ChannelLinkDTO.Modes.TextOnly;
                        message = $@"Link mode for channellink { linkID} is set to 'Text only'";
                    }
                    File.WriteAllText(ConfigHelper.ChannelsLinkedPath, JsonConvert.SerializeObject(CommandHandler.ChannelsLinkedList));
                    await Context.Channel.SendMessageAsync(message);
                }
                else
                {
                    await Context.Channel.SendMessageAsync("Didn't find any links with that ID");

                }

            }
            else
            {
                if (CommandHandler.GuildSettingsList.Exists(x => x.GuildID == Context.Guild.Id))
                {
                    var setting = CommandHandler.GuildSettingsList.Find(x => x.GuildID == Context.Guild.Id);
                    if (setting.LinkMode == GuildSettingsDTO.Modes.TextOnly)
                    {
                        setting.LinkMode = GuildSettingsDTO.Modes.FilesOnly;
                        message = "Link mode set to 'Files only'";

                    }
                    else if (setting.LinkMode == GuildSettingsDTO.Modes.FilesOnly)
                    {
                        setting.LinkMode = GuildSettingsDTO.Modes.PicturesAndFilesOnly;
                        message = "Link mode set to 'Pictures and Files only'";
                    }
                    else if (setting.LinkMode == GuildSettingsDTO.Modes.PicturesAndFilesOnly)
                    {
                        setting.LinkMode = GuildSettingsDTO.Modes.EmbedOnly;
                        message = "Link mode set to 'Embed only'";
                    }
                    else if (setting.LinkMode == GuildSettingsDTO.Modes.EmbedOnly)
                    {
                        setting.LinkMode = GuildSettingsDTO.Modes.NoneEmbedOnly;
                        message = "Link mode set to 'None Embed only'";
                    }
                    else if (setting.LinkMode == GuildSettingsDTO.Modes.NoneEmbedOnly)
                    {
                        setting.LinkMode = GuildSettingsDTO.Modes.All;
                        message = "Link mode set to 'All'";
                    }
                    else
                    {
                        setting.LinkMode = GuildSettingsDTO.Modes.TextOnly;
                        message = "Link mode set to 'Text only'";
                    }
                }
                else
                {

                    var setting = new GuildSettingsDTO();
                    setting.GuildID = Context.Guild.Id;
                    setting.LinkMode = GuildSettingsDTO.Modes.TextOnly;
                    message = "Link mode set to 'Text only'";
                    CommandHandler.GuildSettingsList.Add(setting);
                }
                await Context.Channel.SendMessageAsync(message);

                File.WriteAllText(ConfigHelper.GuildSettingsListPath, JsonConvert.SerializeObject(CommandHandler.GuildSettingsList));
            }

        }

        [Command("sendmsg")]
        [RequireOwner]
        public async Task sendmsg(ulong channelid, [Remainder]string text)
        {
            try
            {
                var chan = Context.Client.GetChannel(channelid);
                var channel = (chan as SocketTextChannel);
                if (channel.Guild.Id == 293791744682622978 || channel.Guild.Id == 241172469891858432)
                {
                    await channel.SendMessageAsync(text);
                }
            }
            catch (Exception)
            {
            }
        }
        [Command("getserverinvite")]
        [RequireOwner]
        public async Task getserverinvite(ulong serveid)
        {
            try
            {
                var guild = Context.Client.GetGuild(serveid);
                var invite = await guild.GetInvitesAsync();


            }
            catch (Exception)
            {
            }
        }
        [Command("sendmessagetomainchannel")]
        [RequireOwner]
        public async Task sendmessagetomainchannel(ulong serveid)
        {
            try
            {
                var guild = Context.Client.GetGuild(serveid);
                await guild.DefaultChannel.SendMessageAsync(guild.Owner.Mention + "Hi, This is the creator of ChannelLinkerBot i have disabled the bot on this server cause of the big traffic.\n Contact me SindreMA#9630, so we can try to get something up just for you guys. \n Plz make sure a admin / mod sees this. ");
                await Program.Log("MESSAGE HAVE BEEN SENT TO DEFAULT CHANNEL!", ConsoleColor.Green);

            }
            catch (Exception)
            {
            }
        }
        [Command("getserverowners")]
        [RequireOwner]
        public async Task getserverowners()
        {
            try
            {
                List<string> owners = new List<string>();
                foreach (var item in Context.Client.Guilds)
                {
                    string be = "";

                    be = be + " - " + item.Name;
                    be = be + " - " + item.Id;
                    be = be + " #### ";
                    be = be + " - " + item.Owner.Id;
                    be = be + " - " + item.Owner.Nickname;
                    be = be + " - " + item.Owner.Username;
                    be = be + " - " + item.Owner.Status;

                    owners.Add(be);
                }
                if (File.Exists(ConfigHelper.OwnersPath))
                {
                    File.Delete(ConfigHelper.OwnersPath);
                }
                File.WriteAllText(ConfigHelper.OwnersPath, JsonConvert.SerializeObject(owners, Formatting.Indented));
                await Context.Channel.SendFileAsync(ConfigHelper.OwnersPath);
            }
            catch (Exception)
            {
            }

        }
        [Command("help")]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        [Alias("?")]
        public async Task help()
        {
            await Context.Channel.SendMessageAsync
                (
                "*(A link is a connection between 1 channal to another, this means that all messages posted in channel A goes also to channel B)*" + Environment.NewLine + Environment.NewLine +
                "**.help** (Shows this message)" + Environment.NewLine +
                "**.ShowLinks** (Returns all the current linked channels in the server)" + Environment.NewLine +
                "**.DeleteLink [FromChannelID] [ToChannelID]** (Deletes the specified link)" + Environment.NewLine +
                "**.CreateLink [FromChannelID] [ToChannelID]** (Creates a link from the first channel to the second channel)" + Environment.NewLine +
                "**.LinkMode** (Lets you set what content its should redirect. You can also pass the link ID to set mode per link)" + Environment.NewLine +
                "**.ResetLinks** (Removes all links and Removes all prefixes)" + Environment.NewLine +
                "**.Prefix [prefix message here]** (Lets you make the bot say something before it repeats the message)" + Environment.NewLine +
                "*(You can use the following keywords to get certan inputs)*\n" +
                "```" +
                "* [_USER_]  = Username of sender\n" +
                "* [USER] = Mention user\n" +
                "* [CHANNEL] = Channel name\n" +
                "* [TITLE] = If the msg it copies is a Embeded message this grabs the title\n" +
                "* [EMBED] = makes the message be embeded\n" +
                "* [_EMBED_] = not sure, think it just redirects embeded messages without any changes\n" +
                "```"
                );
        }
        [Command("resetlinks")]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        public async Task Reset()
        {
            try
            {

                var s = CommandHandler.MessagePrefixList.Remove(CommandHandler.MessagePrefixList.Find(x => x.GuildID == Context.Guild.Id));
                File.WriteAllText(ConfigHelper.ChannelsLinkedPath, JsonConvert.SerializeObject(CommandHandler.ChannelsLinkedList));
                foreach (var item in CommandHandler.ChannelsLinkedList.FindAll(x => x.GuildID == Context.Guild.Id))
                {
                    CommandHandler.ChannelsLinkedList.Remove(item);
                }
                File.WriteAllText(ConfigHelper.ChannelsLinkedPath, JsonConvert.SerializeObject(CommandHandler.ChannelsLinkedList));
                await Context.Channel.SendMessageAsync("All links and prefix have been deleted!");
            }
            catch (Exception)
            {

            }


        }
        [Command("listrooms")]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        public async Task listrooms()
        {
            string msg = "";
            foreach (var item in Context.Guild.TextChannels)
            {
                msg = msg + item.Name + " = " + item.Id + Environment.NewLine;
            }
            await Context.Channel.SendMessageAsync(msg);
        }
        [Command("t-e")]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        public async Task te()
        {
            await Context.Channel.SendMessageAsync("", false, SimpleEmbed(new Color(1f, 1f, 1f), "Testing stuff", "This is a test \n This is a test \n This is a test \n This is a test \n ", "https://images-ext-1.discordapp.net/external/PzuU9ucgl7dbe7aPQXU79WIQzeOakDMFauY6y8iRAQY/https/raw.githubusercontent.com/kvangent/PokeAlarm/master/icons/103.png?width=80&height=80"));
        }
        [Command("sendall")]
        [RequireOwner]
        public async Task sendall([Remainder]string message)
        {
            foreach (var item in Context.Guild.TextChannels)
            {
                if (item.Topic != null)
                {


                    if (item.Topic.ToLower().Contains("_city_"))
                    {
                        Thread.Sleep(500);
                        await item.SendMessageAsync(message);
                    }
                }
            }
        }
        [Command("showlinks")]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        public async Task showlinks()
        {
            string msg = "";
            try
            {
                int i = 0;
                foreach (var item in CommandHandler.ChannelsLinkedList.FindAll(x => x.GuildID == Context.Guild.Id))
                {
                    if (msg.Length > 1700)
                    {
                        System.Threading.Thread.Sleep(500);
                        await Context.Channel.SendMessageAsync(msg);
                        msg = "";
                    }
                    try
                    {
                        msg = msg + $@"[{i}] From =  **" + Context.Guild.GetChannel(item.ChannelCopyFrom).Name + "**(*" + item.ChannelCopyFrom + "*)" + " To = **" + Context.Guild.GetChannel(item.ChannelCopyTo).Name + "**(*" + item.ChannelCopyTo + "*) Mode = **" + item.LinkMode + "**" + Environment.NewLine;
                    }
                    catch (Exception)
                    {
                        msg = msg + $@"[{i}] Error on From =  **" + item.ChannelCopyFrom + "**  To = **" + item.ChannelCopyTo + "** Mode = **" + item.LinkMode + "**" + Environment.NewLine;
                    }
                    i++;

                }
            }
            catch (Exception)
            {
                msg = "There a no links for this guild!";
            }
            if (msg == null || msg == "")
            {
                msg = "There a no links for this guild!";
            }
            await Context.Channel.SendMessageAsync(msg);

        }
        [Command("DeleteLink")]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        [Alias("Removelink")]
        public async Task Removelink(ulong from, ulong to)
        {
            var s = CommandHandler.ChannelsLinkedList.Remove(CommandHandler.ChannelsLinkedList.Find(x => x.GuildID == Context.Guild.Id && x.ChannelCopyFrom == from && x.ChannelCopyTo == to));

            if (s)
            {
                await Context.Channel.SendMessageAsync("Link have been removed!");
                File.WriteAllText(ConfigHelper.ChannelsLinkedPath, JsonConvert.SerializeObject(CommandHandler.ChannelsLinkedList));

            }
            else
            {
                await Context.Channel.SendMessageAsync("Link was not found!");
            }
        }
        [Command("CreateLink")]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        public async Task link(string from, string to)
        {
            ulong uto;
            ulong ufrom;
            if (ulong.TryParse(from.Replace("<#", "").Replace(">",""), out ufrom) && ulong.TryParse(to.Replace("<#", "").Replace(">", ""), out uto))
            {

                if (CommandHandler.ChannelsLinkedList.FindAll(x => x.ChannelCopyFrom == uto && x.GuildID == Context.Guild.Id).Count != 0  || WillCauseLoop(CommandHandler.ChannelsLinkedList.FindAll(x => x.GuildID == Context.Guild.Id),ufrom,uto))
                {
                    await Context.Channel.SendMessageAsync("Link cant create link cause it will cause a loop! Please remove the othe link before adding this.");
                }
                else
                {
                    if (Context.Guild.TextChannels.Any(x => x.Id == ufrom) && Context.Guild.TextChannels.Any(x => x.Id == uto))
                    {

                        CommandHandler.ChannelsLinkedList.Remove(CommandHandler.ChannelsLinkedList.Find(x => x.GuildID == Context.Guild.Id && x.ChannelCopyFrom == ufrom && x.ChannelCopyTo == uto));
                        var Channellink = new ChannelLinkDTO();
                        Channellink.GuildID = Context.Guild.Id;
                        Channellink.ChannelCopyFrom = ufrom;
                        Channellink.ChannelCopyTo = uto;
                        CommandHandler.ChannelsLinkedList.Add(Channellink);
                        await Context.Channel.SendMessageAsync("Link have been saved!");

                        File.WriteAllText(ConfigHelper.ChannelsLinkedPath, JsonConvert.SerializeObject(CommandHandler.ChannelsLinkedList));
                    }
                    else
                    {
                        await Context.Channel.SendMessageAsync("Cant find one of the channels!");
                    }

                }

            }
            else
            {
                await Context.Channel.SendMessageAsync("Failed to parse your input, please double check the command.");
            }
        }
        [Command("prefix")]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        public async Task prefix([Optional][Remainder]string Userprefix)
        {
            CommandHandler.MessagePrefixList.Remove(CommandHandler.MessagePrefixList.Find(x => x.GuildID == Context.Guild.Id));

            if (Userprefix != null)
            {
                MessagePrefixDTO Prefix = new MessagePrefixDTO();
                Prefix.GuildID = Context.Guild.Id;
                Prefix.Prefix = Userprefix;
                CommandHandler.MessagePrefixList.Add(Prefix);
                await Context.Channel.SendMessageAsync("Changes have been saved!");
            }
            else
            {
                await Context.Channel.SendMessageAsync("Prefix deleted!");
            }
            File.WriteAllText(ConfigHelper.MessagePrefixPath, JsonConvert.SerializeObject(CommandHandler.MessagePrefixList));
        }
        public Stream URLToStream(string URL)
        {
            Byte[] dat = null;
            using (var cli = new HttpClient())
            {
                var rslt = cli.GetAsync(URL).GetAwaiter().GetResult(); if (rslt.IsSuccessStatusCode)
                {
                    dat = rslt.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
                }
            }
            return new MemoryStream(dat);
        }
        public static Embed SimpleEmbed(Color c, string title, string description)
        {
            EmbedBuilder eb = new EmbedBuilder();

            eb.WithColor(c);
            eb.Title = title;
            eb.WithDescription(description);


            return eb.Build();
        }

        [Command("info")]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        public async Task Info()
        {
            var application = await Context.Client.GetApplicationInfoAsync();
            await ReplyAsync(
                $"{Format.Bold("Info")}\n" +
                $"- Author: SindreMA#9630\n" +
                $"- Library: Discord.Net ({DiscordConfig.Version})\n" +
                $"- Runtime: {RuntimeInformation.FrameworkDescription} {RuntimeInformation.OSArchitecture}\n" +
                $"- Uptime: {GetUptime()}" + $"{Format.Bold("Stats")}\n" +
                $"- Heap Size: {GetHeapSize()} MB\n" +
                $"- Guilds: {(Context.Client as DiscordSocketClient).Guilds.Count}\n" +
                $"- Channels: {(Context.Client as DiscordSocketClient).Guilds.Sum(g => g.Channels.Count)}\n" +
                $"- Users: {(Context.Client as DiscordSocketClient).Guilds.Sum(g => g.Users.Count)}"
            );
        }
        [Command("setgame")]
        [RequireOwner]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetGame([Remainder]string text)
        {
            await Context.Client.SetGameAsync(text);
        }
        private static string GetUptime()
           => (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString(@"dd\.hh\:mm\:ss");
        private static string GetHeapSize() => Math.Round(GC.GetTotalMemory(true) / (1024.0 * 1024.0), 2).ToString();

        public static Embed SimpleEmbed(Color c, string title, string description, string ImageURL)
        {
            EmbedBuilder eb = new EmbedBuilder();
            eb.WithImageUrl(ImageURL);
            eb.WithColor(c);
            eb.Title = title;
            eb.WithDescription(description);


            return eb.Build();
        }
        public bool WillCauseLoop (List<ChannelLinkDTO> ls, ulong from, ulong to)
        {
            if (from == to)
            {
                return true;
            }
            foreach (var i in ls)
            {
                if (i.ChannelCopyTo == from)
                {
                    return true;
                }
                if (i.ChannelCopyFrom == to)
                {
                    return true;
                }
            }

            return false;
        }
    }
}